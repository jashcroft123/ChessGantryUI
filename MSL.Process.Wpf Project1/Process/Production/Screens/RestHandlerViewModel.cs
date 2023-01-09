using HelixToolkit.Wpf.SharpDX;
using MSL.Core;
using MSL.Core.Wpf;
using MSL.Process.Wpf_Project1.Game;
using MSL.Process.Wpf_Project1.Models;
using MSL.Process.Wpf_Project1.Models.Piece;
using MSL.Process.Wpf_Project1.Process.Shared;
using Prism.Mvvm;
using SharpDX;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Camera = HelixToolkit.Wpf.SharpDX.Camera;
using PerspectiveCamera = HelixToolkit.Wpf.SharpDX.PerspectiveCamera;
using Point3D = System.Windows.Media.Media3D.Point3D;
using Transform3D = System.Windows.Media.Media3D.Transform3D;
using TranslateTransform3D = System.Windows.Media.Media3D.TranslateTransform3D;
using Vector3D = System.Windows.Media.Media3D.Vector3D;

namespace RESTHandler.Process.Screens
{

    //https://github.com/helix-toolkit/helix-toolkit/wiki
    public class RestHandlerViewModel : BindableBase
    {
        #region Bindable Properties
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

        //private YGantry _YGantry = new YGantry();
        //public YGantry YGantry
        //{
        //    get { return _YGantry; }
        //    set { SetProperty(ref _YGantry, value); }
        //}

        private GameManager _GameManager;
        public GameManager GameManager
        {
            get { return _GameManager; }
            set { SetProperty(ref _GameManager, value); }
        }

        private Element3D _Target;
        public Element3D Target
        {
            get { return _Target; }
            set { SetProperty(ref _Target, value); }
        }

        private Vector3 _TargetCentre;
        public Vector3 TargetCentre
        {
            get { return _TargetCentre; }
            set { SetProperty(ref _TargetCentre, value); }
        }
        #endregion

        #region Delegate Commands

        #endregion

        #region Public Properties
        //not actually the board centre probably should figure this one out
        public Transform3D Origin { private set; get; } = new TranslateTransform3D(0, 0, 0);
        public PhongMaterial Red { get; set; } = PhongMaterials.Red;
        public Geometry3D OriginSphere { get; set; }
        public BitmapSource UndoImage { get; set; }
        public BitmapSource RedoImage { get; set; }
        #endregion

        #region Private Properties
        private static ConcurrentDictionary<string, BitmapImage> _CachedImages;
        #endregion


        public RestHandlerViewModel(GameManager gameManager)
        {
            var meshBuilder = new MeshBuilder(true, true, true);
            meshBuilder.AddSphere(new Vector3(0, 0, 0), 5);
            OriginSphere = meshBuilder.ToMeshGeometry3D();

            GameManager = gameManager;
            EffectsManager = new DefaultEffectsManager();

            //Camera = new OrthographicCamera
            Camera = new PerspectiveCamera
            {
                Position = new Point3D(SharedConstants.BoardCentre, 240 / 3, SharedConstants.BoardCentre), //{359.026184082031,281.295013427734,337.112075805664}
                LookDirection = new Vector3D(-1, -1, -1), //{-239.2578125,-290.496185302734,-207.983459472656}
                UpDirection = new Vector3D(0, 1, 0),
                FarPlaneDistance = 10000,
                NearPlaneDistance = 1
            };

            UndoImage = ImageFromPath("Resources.Undo.png");
            RedoImage = ImageFromPath("Resources.Redo.png");
        }

        public static BitmapImage ImageFromPath(string path, Assembly assembly = null)
        {
            BitmapImage img = new BitmapImage();

            if (_CachedImages is null) _CachedImages = new ConcurrentDictionary<string, BitmapImage>();

            if (_CachedImages.ContainsKey(path))
                return _CachedImages[path];

            if (assembly == null) assembly = Assembly.GetExecutingAssembly();

            path = $"{assembly.GetName().Name}.{path}";

            using (Stream stm = assembly.GetManifestResourceStream(path))
            {
                img.BeginInit();
                img.StreamSource = stm ?? throw new MslException(984191, "Component image not found in code assembly.", new FileNotFoundException(path));
                img.EndInit();
                _CachedImages.TryAdd(path, img);
            }
            return img;
        }

        #region Private Methods
        public void OnMouseUp3DHandler(object sender, MouseUp3DEventArgs e)
        {
            if (e.HitTestResult != null && e.HitTestResult.ModelHit is MeshGeometryModel3D m)
            {
                if (m.DataContext is PieceBase piece)
                {
                    GameManager.PieceSelected(piece);
                }

                else if (m.DataContext is BoardSquare square)
                {
                    GameManager.SquareSelected(square);
                }

                Target = null;
                TargetCentre = m.Geometry.Bound.Center; // Must update this before updating target
                Target = e.HitTestResult.ModelHit as Element3D;
            }
        }
        #endregion
    }
}
