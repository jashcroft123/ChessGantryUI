using HelixToolkit.Wpf.SharpDX;
using MSL.Core;
using MSL.Process.Wpf_Project1.Process.Shared;
using SharpDX.Direct2D1;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using MeshGeometry3D = HelixToolkit.Wpf.SharpDX.MeshGeometry3D;

namespace MSL.Process.Wpf_Project1.Models
{
    public class ModelBase : MslBindableBase
    {

        #region Bindable Properties
        private Transform3D _Transform = new TranslateTransform3D(0, 0, 0);
        public Transform3D Transform
        {
            get { return _Transform; }
            set { SetProperty(ref _Transform, value); }
        }

        private MeshGeometry3D _Geometry;
        public MeshGeometry3D Geometry
        {
            get { return _Geometry; }
            set { SetProperty(ref _Geometry, value); }
        }


        private PhongMaterial _Material;
        public PhongMaterial Material
        {
            get { return _Material; }
            set { SetProperty(ref _Material, value); }
        }

        public PhongMaterial InitialMaterial { get; protected set; }
        #endregion


        #region Public Properties
        public virtual PhongMaterial SelctionColour { get; set; } = PhongMaterials.Red;
        #endregion

        #region Private Properties

        #endregion


        private static ConcurrentDictionary<string, List<Object3D>> ObjDict = new ConcurrentDictionary<string, List<Object3D>>();

        public void LoadDictionary()
        {
            if (ObjDict.Count > 0)
                return;

            string[] objFiles = Directory.GetFiles($"Models/Objs/");
            string[] stlFiles = Directory.GetFiles($"Models/Stls/");

            string[] files = objFiles.Concat(stlFiles).ToArray();

            //parallel load all the model files needed
            Parallel.ForEach<string>(files, file =>
            {
                LoadObj(file.ToLower());
            });
        }

        public List<Object3D> LoadObj(string path)
        {
            List<Object3D> list;

            if (ObjDict.TryGetValue(path.ToLower(), out list))
                return list;

            if (path.EndsWith(".obj", StringComparison.CurrentCultureIgnoreCase))
            {
                var reader = new ObjReader();
                list = reader.Read(path);
            }
            else if (path.EndsWith(".stl", StringComparison.CurrentCultureIgnoreCase))
            {
                var reader = new StLReader();
                list = reader.Read(path);
            }
            else
            {
                return new List<Object3D>();
            }

            ObjDict.TryAdd(path.ToLower(), list);
            return list;
        }

        /// <summary>
        /// Relative move from current location
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public async Task RelativeTranslate(double x = 0, double y = 0, double z = 0)
        {
            SharedConstants.UIThreadContext.Post(new SendOrPostCallback((object o) =>
            {
                Matrix3D mm = Transform.Value;
                mm.Translate(new Vector3D(x, y, z));
                Transform = new MatrixTransform3D(mm);
            }), null);
        }

        /// <summary>
        /// Set absolute location of geometry
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public async Task AbsoluteTranslate(double x = 0, double y = 0, double z = 0)
        {
            SharedConstants.UIThreadContext.Post(new SendOrPostCallback((object o) =>
            {
                Matrix3D mm = Transform.Value;
                mm.Translate(new Vector3D(x - mm.OffsetX, y - mm.OffsetY, z - mm.OffsetZ));
                Transform = new MatrixTransform3D(mm);
            }), null);
        }

        public async Task Rotate(double angle, string axis = "y", bool aroundModelCentre = true)
        {
            double x = 0;
            double y = 0;
            double z = 0;

            switch (axis.ToLower())
            {
                case "x":
                    x = 1;
                    break;

                case "y":
                    y = 1;
                    break;

                case "z":
                    z = 1;
                    break;
            }

            SharedConstants.UIThreadContext.Post(new SendOrPostCallback((object o) =>
            {
                Matrix3D mm = Transform.Value;

                if (aroundModelCentre)
                    mm.RotateAt(new Quaternion(new Vector3D(x, y, z), angle), new Point3D(Transform.Value.OffsetX, Transform.Value.OffsetY, Transform.Value.OffsetZ));
                else
                    mm.Rotate(new Quaternion(new Vector3D(x, y, z), angle));

                Transform = new MatrixTransform3D(mm);
            }), null);
        }

        public async Task Scale(double x = 0, double y = 0, double z = 0)
        {
            Matrix3D mm = Transform.Value;
            mm.Scale(new Vector3D(x, y, z));
            Transform = new MatrixTransform3D(mm);
        }

        public ModelBase()
        {
            LoadDictionary();
        }
    }
}
