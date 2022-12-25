using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel; // INotifyPropertyChanged

namespace RebornAvatar.IO.Models
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public EffectsManager EffectsManager { get; }

        public Camera Camera { get; }

        public Geometry3D CubeMesh { get; }

        public Material Red { get; }

        public MainViewModel()
        {
            EffectsManager = new DefaultEffectsManager();
            Camera = new PerspectiveCamera();
            var builder = new MeshBuilder();
            builder.AddCube();
            CubeMesh = builder.ToMesh();
            Red = PhongMaterials.Red;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string info = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        protected bool Set<T>(ref T backingField, T value, [CallerMemberName] string propertyName = "")
        {
            if (object.Equals(backingField, value))
            {
                return false;
            }

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion
    }
}
