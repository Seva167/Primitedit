using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PrimitierSaveEditor.Entities;
using HelixToolkit.Wpf.SharpDX;
using System.Windows.Media.Media3D;
using System.Diagnostics;
using System.Collections;
using PrimitierSaveEditor.Entities.Primitier;
using PrimitierSaveEditor.Controllers;
using System.ComponentModel;

namespace PrimitierSaveEditor
{
    public partial class MainWindow : Window
    {
        const string dialogFilter = "Primitier Save file (*.dat)|*.dat|Primitier Uncompressed save file (*.json)|*.json|Any File (*.*)|*.*";

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < 7; i++)
            {
                viewport.InputBindings.RemoveAt(0);
            }

            UpdateMemLoop();
            AutoSaveLoop();
            CheckForUpdates();
        }

        string editFilename = null;

        private async void UpdateMemLoop()
        {
            while (true)
            {
                Process proc = Process.GetCurrentProcess();
                memLabel.Content = $"Memory: {proc.PrivateMemorySize64 / 1024 / 1024}M/{GC.GetGCMemoryInfo().TotalAvailableMemoryBytes / 1024 / 1024}M";
                await Task.Delay(1000);
            }
        }

        private async void AutoSaveLoop()
        {
            while (true)
            {
                await Task.Delay(SettingsController.AppSettings.AutoSaveInterval * 60 * 100);

                if (SettingsController.AppSettings.AutoSaveEnabled && editFilename != null)
                    TrySave(false);
            }
        }

        private async void CheckForUpdates()
        {
            if (!SettingsController.AppSettings.AutoUpdateCheck)
                return;

            UpdateController.UpdateInfo info = await UpdateController.GetUpdateInfo();
            if (info == null || !info.IsNewerVersion())
                return;

            UpdateWindow window = new UpdateWindow(info.Url, info.Tag, info.Name, info.Body)
            {
                Owner = this
            };

            window.ShowDialog();
        }

        private void AboutPress(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow
            {
                Owner = this
            };
            aboutWindow.ShowDialog();
        }

        private void OpenWorldSettings(object sender, RoutedEventArgs e)
        {
            WorldSettings playerSettings = new WorldSettings
            {
                Owner = this
            };
            playerSettings.Show();
        }

        private void MenuOpenExecuted(object sender, ExecutedRoutedEventArgs e) => openMenu.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
        private void MenuSaveExecuted(object sender, ExecutedRoutedEventArgs e) => saveMenu.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
        private void MenuSaveAsExecuted(object sender, ExecutedRoutedEventArgs e) => saveAsMenu.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));

        private void MenuOpenClick(object sender, RoutedEventArgs e)
        {
            if (AskUnsavedChanges())
                return;

            OpenFileDialog openDialog = new OpenFileDialog()
            {
                Filter = dialogFilter
            };

            if (openDialog.ShowDialog().Value)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;

                    try
                    {
                        SaveController.OpenSaveFile(openDialog.FileName);
                    }
                    catch (System.IO.IOException ex)
                    {
                        MessageBox.Show(ex.Message, "Save reading error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Logger.LogError($"Save reading error: {ex.Message}{ex.StackTrace}");
                        return;
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        MessageBox.Show(ex.Message, "Save reading error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Logger.LogError($"Save reading error: {ex.Message}{ex.StackTrace}");
                        return;
                    }
                    catch (System.IO.InvalidDataException ex)
                    {
                        MessageBox.Show("Error decompressing save", "Save reading error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Logger.LogError($"Error decompressing save: {ex.Message}{ex.StackTrace}");
                        return;
                    }
                    catch (Newtonsoft.Json.JsonException ex)
                    {
                        MessageBox.Show($"Error processing save json data: {ex.Message}", "Save reading error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Logger.LogError($"Error processing save json: {ex.Message}{ex.StackTrace}");
                        return;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Unknown error while reading save: {ex.Message}", "Save reading error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Logger.LogError($"Unknown save reading error: {ex.Message}{ex.StackTrace}");
                        return;
                    }

                    if (SaveController.Save == null || SaveController.Save.version == null)
                    {
                        MessageBox.Show("Invalid save data", "Save reading error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Logger.LogError($"Save data is invalid");
                        return;
                    }

                    editFilename = openDialog.FileName;
                    Title = $"Editing {System.IO.Path.GetFileName(editFilename)}";
                    worldSettingsMenu.IsEnabled = true;
                    showWaterCheck.IsEnabled = true;

                    SelectionController.Selection = null;

                    viewport.Items.Clear();
                    viewport.InvalidateSceneGraph();

                    GC.Collect(0, GCCollectionMode.Forced, true);

                    viewport.Items.Add(TerrainController.CreateWater());
                    foreach (var terrain in TerrainController.CreateTerrain())
                        viewport.Items.Add(terrain);
                    foreach (var chunk in ChunkController.CreateChunks())
                        viewport.Items.Add(chunk);
                    viewport.Items.Add(PlayerController.CreatePlayer());
                    viewport.Items.Add(PlayerController.CreateRespawnPos());
                    viewport.Items.Add(PlayerController.CreateCamera());

                    viewport.Camera.Position = SaveController.Save.saveEditorMetadata.cameraPos;
                    viewport.Camera.LookDirection = SaveController.Save.saveEditorMetadata.cameraDir;
                    viewport.Camera.UpDirection = new Vector3D(0, 1, 0);

                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            }
        }

        private void MenuSaveClick(object sender, RoutedEventArgs e) => TrySave(false);

        private void MenuSaveAsClick(object sender, RoutedEventArgs e) => TrySave(true);

        private bool TrySave(bool forceDialog)
        {
            if (SaveController.Save == null)
                return false;

            SaveFileDialog saveDialog = new SaveFileDialog()
            {
                Filter = dialogFilter
            };

            if ((editFilename != null && !forceDialog) || saveDialog.ShowDialog().Value)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;

                    string filename;
                    try
                    {
                        filename = editFilename == null || forceDialog ? saveDialog.FileName : editFilename;
                        SaveController.SaveSaveFile(filename);
                    }
                    catch (System.IO.IOException ex)
                    {
                        MessageBox.Show(ex.Message, "Save writing error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Logger.LogError($"Save writing error: {ex.Message}{ex.StackTrace}");
                        return false;
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        MessageBox.Show(ex.Message, "Save writing error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Logger.LogError($"Save writing error: {ex.Message}{ex.StackTrace}");
                        return false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Unknown error while writing save: {ex.Message}", "Save writing error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Logger.LogError($"Unknown save writing error: {ex.Message}{ex.StackTrace}");
                        return false;
                    }

                    editFilename = filename;
                    Title = $"Editing {System.IO.Path.GetFileName(editFilename)}";
                    return true;
                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            }

            return false;
        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            VisualProperty property = (sender as Button).DataContext as VisualProperty;

            IList list = (IList)property.Value;

            CollectionEditor collEditor = new CollectionEditor(list)
            {
                Owner = this
            };

            collEditor.Show();
        }

        private void CubeToolClick(object sender, RoutedEventArgs e) => SelectionController.Tool = SelectionTool.Cube;
        private void GroupToolClick(object sender, RoutedEventArgs e) => SelectionController.Tool = SelectionTool.Group;
        private void ChunkToolClick(object sender, RoutedEventArgs e) => SelectionController.Tool = SelectionTool.Chunk;
        private void TerrainToolClick(object sender, RoutedEventArgs e) => SelectionController.Tool = SelectionTool.Terrain;

        private void TerrainNormalViewClick(object sender, RoutedEventArgs e) => TerrainController.TerrainDisplayChange(TerrainView.Normal);
        private void TerrainTemperatureViewClick(object sender, RoutedEventArgs e) => TerrainController.TerrainDisplayChange(TerrainView.Temperature);
        private void TerrainRainfallViewClick(object sender, RoutedEventArgs e) => TerrainController.TerrainDisplayChange(TerrainView.Rainfall);

        PrimitierCube prevSel = null;
        private void CreateBlockClick(object sender, RoutedEventArgs e)
        {
            if (SelectionController.Selection == null || !(SelectionController.Selection is PrimitierCube))
                return;

            PrimitierCube cubeSel = SelectionController.Selection as PrimitierCube;

            if (prevSel == null)
            {
                Vector3D cameraPos = new Vector3D(viewport.Camera.Position.X, viewport.Camera.Position.Y, viewport.Camera.Position.Z) + viewport.Camera.LookDirection;
                Vector3D pos = Utils.WorldPosToCubePos(cameraPos, cubeSel.Group.Transform.ToVector3D(), cubeSel.Group.Chunk.Transform.ToVector3D());

                // Create save objects
                CubeData cubeData = new CubeData
                {
                    pos = pos,
                    rot = Entities.Quaternion.Identity,
                    scale = new Entities.Vector3(0.3f, 0.3f, 0.3f),
                    lifeRatio = 1,
                    temperature = 300,
                    connections = new List<int>(),
                    behaviors = new List<string>(),
                    states = new List<string>()
                };

                // Add to the save object
                cubeSel.Group.Data.cubes.Add(cubeData);

                // Add to visual scene
                PrimitierCube cube = new PrimitierCube(cubeData, cubeSel.Group);
                cube.Group.Children.Add(cube);

                // Refresh and select
                viewport.InvalidateSceneGraph();

                SelectionController.Selection = null;

                // Enter connection mode
                prevSel = cube;
                addCubeConnLabel.Content = "Select cube to connect to and press the same button\nPress ESC to cancel";
            }
            else
            {
                // Add target index to source 'connections' list
                for (int i = 0; i < prevSel.Group.Data.cubes.Count; i++)
                {
                    if (prevSel.Group.Data.cubes[i] == cubeSel.Data)
                    {
                        prevSel.Data.connections.Add(i);
                        addCubeConnLabel.Content = "Connected";
                        ClearTextInTime(addCubeConnLabel, 1000);
                        prevSel = null;
                        return;
                    }
                }

                // Didn't found anything
                addCubeConnLabel.Content = "Group mismatch, choose cube's own group";
            }
        }

        private async void ClearTextInTime(Label label, int time)
        {
            await Task.Delay(time);
            label.Content = null;
        }

        private void CreateGroupClick(object sender, RoutedEventArgs e)
        {
            if (SelectionController.Selection == null || !(SelectionController.Selection is PrimitierCube))
                return;

            PrimitierCube cubeSel = SelectionController.Selection as PrimitierCube;

            // Create save objects
            CubeData cubeData = new CubeData
            {
                rot = Entities.Quaternion.Identity,
                scale = new Entities.Vector3(1, 1, 1),
                lifeRatio = 1,
                temperature = 300,
                connections = new List<int>(),
                behaviors = new List<string>(),
                states = new List<string>()
            };

            SaveData.ChunkData.GroupData group = new SaveData.ChunkData.GroupData
            {
                cubes = new List<CubeData>() { cubeData },
                pos = new Entities.Vector3(0, 0, 0),
                rot = Entities.Quaternion.Identity
            };

            PrimitierChunk currentChunk = cubeSel.Group.Chunk;

            // Add to the save object
            Vector3D cameraPos = new Vector3D(viewport.Camera.Position.X, viewport.Camera.Position.Y - 1, viewport.Camera.Position.Z) + viewport.Camera.LookDirection;
            group.pos = Utils.WorldPosToCubePos(cameraPos, group.pos, currentChunk.Transform.ToVector3D());

            currentChunk.Data.groups.Add(group);
            IsDirty = true;

            // Add to visual scene
            PrimitierGroup primGroup = new PrimitierGroup(group, currentChunk);
            PrimitierCube cube = new PrimitierCube(cubeData, primGroup);

            currentChunk.Children.Add(primGroup);
            primGroup.Children.Add(cube);

            // Refresh and select
            viewport.InvalidateSceneGraph();

            SelectionController.Tool = SelectionTool.Cube;
            SelectionController.Selection = cube;
        }

        private void DeleteBlockClick(object sender, RoutedEventArgs e)
        {
            if (SelectionController.Selection == null || !(SelectionController.Selection is PrimitierCube))
                return;

            PrimitierCube cubeSel = SelectionController.Selection as PrimitierCube;

            // Remove from visual and save
            cubeSel.Group.Children.Remove(cubeSel);
            viewport.InvalidateSceneGraph();
            cubeSel.Group.Data.cubes.Remove(cubeSel.Data);

            IsDirty = true;

            // If no cubes left, destroy group
            if (cubeSel.Group.Data.cubes.Count == 0)
                cubeSel.Group.Chunk.Data.groups.Remove(cubeSel.Group.Data);

            SelectionController.Selection = null;
        }

        private void DeleteGroupClick(object sender, RoutedEventArgs e)
        {
            if (SelectionController.Selection == null || !(SelectionController.Selection is PrimitierCube))
                return;

            PrimitierCube cubeSel = SelectionController.Selection as PrimitierCube;

            // Remove from visual and save
            cubeSel.Group.Chunk.Children.Remove(cubeSel.Group);
            viewport.InvalidateSceneGraph();
            cubeSel.Group.Chunk.Data.groups.Remove(cubeSel.Group.Data);

            IsDirty = true;

            SelectionController.Selection = null;
        }

        private void ShowWaterChanged(object sender, RoutedEventArgs e)
        {
            if (TerrainController.Water == null)
                return;

            CheckBox cb = sender as CheckBox;
            TerrainController.Water.IsRendering = cb.IsChecked.Value;

            viewport.InvalidateSceneGraph();
        }

        private void CameraToSelected(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectionController.Selection == null || !(SelectionController.Selection is PrimitierCube))
                return;

            PrimitierCube cubeSel = SelectionController.Selection as PrimitierCube;

            Controllers.CameraController.SetCameraToCube(cubeSel);
        }

        private void CameraToPlayer(object sender, ExecutedRoutedEventArgs e)
        {
            if (PlayerController.Player == null)
                return;

            Controllers.CameraController.SetCameraToPlayer(PlayerController.Player);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!viewport.IsFocused)
                return;

            // Inverted A/D movement fix
            switch (e.Key)
            {
                case Key.A:
                    viewport.AddMoveForce(0.2, 0, 0);
                    break;
                case Key.D:
                    viewport.AddMoveForce(-0.2, 0, 0);
                    break;
                case Key.Escape:
                    prevSel = null;
                    addCubeConnLabel.Content = null;
                    break;
            }
        }

        protected override void OnClosing(CancelEventArgs e) => e.Cancel = AskUnsavedChanges();

        public bool IsDirty
        {
            get => _dirty;
            set
            {
                if (SaveController.Save != null)
                    _dirty = value;

                if (_dirty)
                {
                    if (Title[^1] != '*')
                        Title += '*';
                }
                else
                {
                    if (Title[^1] == '*')
                        Title = Title[..^1];
                }
            }
        }
        private bool _dirty = false;

        private bool AskUnsavedChanges()
        {
            if (!IsDirty)
                return false;

            MessageBoxResult closeMessage = MessageBox.Show("You have unsaved changes.\nSave before exiting?", "Exit?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            switch (closeMessage)
            {
                case MessageBoxResult.Yes:
                    bool saveRes = TrySave(false);
                    return !saveRes;
                case MessageBoxResult.No:
                    return false;
                default:
                    return true;
            }
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            Settings settingsWindow = new Settings()
            {
                Owner = this
            };

            settingsWindow.ShowDialog();
        }

        private void CheckForUpdatesClick(object sender, RoutedEventArgs e) => CheckForUpdates();

        private void HowToUseClick(object sender, RoutedEventArgs e) => Utils.OpenLink("https://github.com/Seva167/Primitedit/wiki");

        private void ReportIssueClick(object sender, RoutedEventArgs e) => Utils.OpenLink("https://github.com/Seva167/Primitedit/issues");
    }
}
