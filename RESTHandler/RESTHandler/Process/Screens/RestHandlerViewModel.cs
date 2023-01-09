using MSL.Core.Wpf;
using MSL.Process;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using System.IO;
using System.Collections.ObjectModel;
using HelixToolkit.Wpf.SharpDX.Animations;
using HelixToolkit.Wpf.SharpDX.Controls;
using HelixToolkit.Wpf.SharpDX.Model.Scene;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Assimp;
using SharpDX;

namespace RESTHandler.Process.Screens
{

    //https://github.com/helix-toolkit/helix-toolkit/wiki
    public class RestHandlerViewModel : ViewModelBase
    {
        #region Bindable Properties
        private string _Response = "Start";
        public string Response
        {
            get { return _Response; }
            set { SetProperty(ref _Response, value); }
        }


        private EffectsManager _EffectsManager;
        public EffectsManager EffectsManager
        {
            get { return _EffectsManager; }
            set { SetProperty(ref _EffectsManager, value); }
        }

        private Camera _Camera;
        public Camera Camera
        {
            get { return _Camera; }
            set { SetProperty(ref _Camera, value); }
        }

        private Geometry3D _CubeMesh;
        public Geometry3D CubeMesh
        {
            get { return _CubeMesh; }
            set { SetProperty(ref _CubeMesh, value); }
        }

        private Material _Red;
        public Material Red
        {
            get { return _Red; }
            set { SetProperty(ref _Red, value); }
        }
        #endregion

        #region Delegate Commands

        #endregion

        #region Public Properties
        public SceneNodeGroupModel3D ModelGroup { get; } = new SceneNodeGroupModel3D();
        #endregion

        #region Private Properties
        private HelixToolkitScene scene;
        #endregion


        public RestHandlerViewModel()
        {
            EffectsManager = new DefaultEffectsManager();
            Camera = new PerspectiveCamera();
            var builder = new MeshBuilder();
            builder.AddCube();
            CubeMesh = builder.ToMesh();
            Red = PhongMaterials.Red;


            LoadObj(); //Load obj's into model group
        }

        public override async Task Initialise(object[] initialisationMetadata)
        {
            //await Task.Delay(10000);
        }

        #region Private Methods
        private void LoadObj()
        {
            var importer = new Importer();
            importer.Configuration.CreateSkeletonForBoneSkinningMesh = true;
            importer.Configuration.SkeletonSizeScale = 0.04f;
            importer.Configuration.GlobalScale = 0.1f;
            scene = importer.Load("Resources/Objects/Extrusion.obj");
            ModelGroup.AddNode(scene.Root);
            //Animations = scene.Animations.Select(x => x.Name).ToArray();
            foreach (var node in scene.Root.Items.Traverse(false))
            {
                if (node is BoneSkinMeshNode m)
                {
                    if (!m.IsSkeletonNode)
                    {
                        m.IsThrowingShadow = true;
                        m.WireframeColor = new SharpDX.Color4(0, 0, 1, 1);
                        //boneSkinNodes.Add(m);
                        //m.MouseDown += M_MouseDown;
                    }
                    else
                    {
                        //skeletonNodes.Add(m);
                        m.Visible = false;
                    }
                }
            }
        }

        //private void OpenFile()
        //{
        //    if (IsLoading)
        //    {
        //        return;
        //    }
        //    string path = "Resources/Objects/Extrusion.obj";

        //    var syncContext = SynchronizationContext.Current;
        //    IsLoading = true;
        //    Task.Run(() =>
        //    {
        //        var loader = new Importer();
        //        var scene = loader.Load(path);
        //        scene.Root.Attach(EffectsManager); // Pre attach scene graph
        //        scene.Root.UpdateAllTransformMatrix();
        //        if (scene.Root.TryGetBound(out var bound))
        //        {
        //            /// Must use UI thread to set value back.
        //            syncContext.Post((o) => { ModelBound = bound; }, null);
        //        }
        //        if (scene.Root.TryGetCentroid(out var centroid))
        //        {
        //            /// Must use UI thread to set value back.
        //            syncContext.Post((o) => { ModelCentroid = centroid.ToPoint3D(); }, null);
        //        }
        //        return scene;
        //    }).ContinueWith((result) =>
        //    {
        //        IsLoading = false;
        //        if (result.IsCompleted)
        //        {
        //            scene = result.Result;
        //            Animations.Clear();
        //            var oldNode = GroupModel.SceneNode.Items.ToArray();
        //            GroupModel.Clear(false);
        //            Task.Run(() =>
        //            {
        //                foreach (var node in oldNode)
        //                { node.Dispose(); }
        //            });
        //            if (scene != null)
        //            {
        //                if (scene.Root != null)
        //                {
        //                    foreach (var node in scene.Root.Traverse())
        //                    {
        //                        if (node is MaterialGeometryNode m)
        //                        {
        //                            //m.Geometry.SetAsTransient();
        //                            if (m.Material is PBRMaterialCore pbr)
        //                            {
        //                                pbr.RenderEnvironmentMap = RenderEnvironmentMap;
        //                            }
        //                            else if (m.Material is PhongMaterialCore phong)
        //                            {
        //                                phong.RenderEnvironmentMap = RenderEnvironmentMap;
        //                            }
        //                        }
        //                    }
        //                }
        //                GroupModel.AddNode(scene.Root);
        //                if (scene.HasAnimation)
        //                {
        //                    var dict = scene.Animations.CreateAnimationUpdaters();
        //                    foreach (var ani in dict.Values)
        //                    {
        //                        Animations.Add(ani);
        //                    }
        //                }
        //                foreach (var n in scene.Root.Traverse())
        //                {
        //                    n.Tag = new AttachedNodeViewModel(n);
        //                }
        //                FocusCameraToScene();
        //            }
        //        }
        //        else if (result.IsFaulted && result.Exception != null)
        //        {
        //            MessageBox.Show(result.Exception.Message);
        //        }
        //    }, TaskScheduler.FromCurrentSynchronizationContext());
        //}
        #endregion
    }
}
