using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelixToolkit.Wpf;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Media3D = System.Windows.Media.Media3D;
using System.Diagnostics;
using System.Collections.ObjectModel;
using PropertyTools.Wpf;
using Scene3DViewModelLib;
using Scene3DLib;
using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.JScript;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using System.IO;

namespace Scene3DLib
{
    public class Scene3DObjLoader
    {
        protected static Object lockObject = new Object();
        protected Dictionary<string, Model3D> m_loadedOjbs = new Dictionary<string, Model3D>();
        protected static Scene3DObjLoader scene3DObjLoader = null;

        private Scene3DObjLoader()
        {

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Scene3DObjLoader GetInstance()
        {
            if (scene3DObjLoader != null)
                return scene3DObjLoader;

            lock (Scene3DObjLoader.lockObject)  //synchronize with lock too...
            {
                scene3DObjLoader = new Scene3DObjLoader();
            }

            return scene3DObjLoader;
        }

        public Model3D LoadOjbect(string path)
        {
            Model3D o = null;
            if (this.m_loadedOjbs.TryGetValue(path, out o))
            {
                //ATTENTIONE: cannot insert the same object twice in the same Viewport! -> TODO: clone!

                //ATTENTIONE: ModelVisual3D is not thread safe, Model3D only if .Freeze!
                //ModelVisual3D onew = new ModelVisual3D();
                //onew.Content = o.Content.Clone();
                //return onew;

                return o;
            }

            bool useImporter = true;
            if ( useImporter)
            {
                o = Scene3DLib.Scene3D.getModel(path);
            }
            else
            {
                //ATTENTIONE: does not work with .obj, only with .3ds

                if((path.Length < 2) || (path.ElementAt(1) != ':'))
                {
                    path = Scene3D.AssemblyDirectory + "/" + path;
                    path = path.Replace("/", "\\");
                }
                FileStream c = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                var reader = new StudioReader();
                reader.TexturePath = ".";
                o = reader.Read(c);
            }
            o.Freeze();
            this.m_loadedOjbs[path] = o;
            return o;
        }

        public async Task<Model3D> LoadObjectAsync(string path)
        {
            return LoadOjbect(path);
        }
        public async Task<Model3D> LoadObjectWithTaskAsync(string path)
        {
            return await Task.Run(() =>
            {
                return LoadOjbect(path);
            });
        }
        public void LoadObject2ActionAsync(string path, Action<Model3D> onLoaded)
        {
            Task.Run(() =>
            {
                onLoaded.Invoke(LoadOjbect(path));
            });
        }
        public void LoadObject2FuncAsync(string path, Func<Model3D, Model3D> onLoaded)
        {
            Task.Run(() =>
            {
                Model3D o = this.LoadOjbect(path);
                onLoaded.Invoke(o);
            });
        }
    }
}
