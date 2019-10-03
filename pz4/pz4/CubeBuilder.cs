using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace pz4
{
    public class VectorHelper
    {
        //za trazenje normale 2 vektora
        public static Vector3D CalcNormal(Point3D p0, Point3D p1, Point3D p2)
        {
            Vector3D v0 = new Vector3D(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
            Vector3D v1 = new Vector3D(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
            return Vector3D.CrossProduct(v0, v1);
        }
    }
    public class CubeBuilder
    {
        private Color _col;
        
        public CubeBuilder (Color color)
        {
            _col = color;
        }

        public ModelVisual3D Create(double size, double x, double y, double z, string info)
        {
            Model3DGroup cube = new Model3DGroup();

            //efektivno tacke kocke
            Point3D p0 = new Point3D(0 + x, 0 + y, 0 + z);
            Point3D p1 = new Point3D(size + x, 0 + y, 0 + z);
            Point3D p2 = new Point3D(size + x, 0 + y, size + z);
            Point3D p3 = new Point3D(0 + x, 0 + y, size + z);
            Point3D p4 = new Point3D(0 + x, size + y, 0 + z);
            Point3D p5 = new Point3D(size + x, size + y, 0 + z);
            Point3D p6 = new Point3D(size + x, size + y, size + z);
            Point3D p7 = new Point3D(0 + x, size + y, size + z);

            //front
            cube.Children.Add(CreateTriangle(p3, p2, p6));
            cube.Children.Add(CreateTriangle(p3, p6, p7));

            //right
            cube.Children.Add(CreateTriangle(p2, p1, p5));
            cube.Children.Add(CreateTriangle(p2, p5, p6));

            //back
            cube.Children.Add(CreateTriangle(p1, p0, p4));
            cube.Children.Add(CreateTriangle(p1, p4, p5));

            //left
            cube.Children.Add(CreateTriangle(p0, p3, p7));
            cube.Children.Add(CreateTriangle(p0, p7, p4));

            //top
            cube.Children.Add(CreateTriangle(p7, p6, p5));
            cube.Children.Add(CreateTriangle(p7, p5, p4));

            //bottom
            cube.Children.Add(CreateTriangle(p2, p3, p0));
            cube.Children.Add(CreateTriangle(p2, p0, p1));

            ModelVisual3D model = new ModelVisual3D();
            model.Content = cube;
            model.Transform = Helper.group;
            Helper.models.Add(model);
            Helper.tooltips.Add(info);
            return model;
        }

        public Model3DGroup CreateTriangle(Point3D p0, Point3D p1, Point3D p2)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions.Add(p0);
            mesh.Positions.Add(p1);
            mesh.Positions.Add(p2);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);

            Vector3D normal = VectorHelper.CalcNormal(p0, p1, p2);
            mesh.Normals.Add(normal);
            mesh.Normals.Add(normal);
            mesh.Normals.Add(normal);

            Material material = new DiffuseMaterial(new SolidColorBrush(_col));
            GeometryModel3D model = new GeometryModel3D(mesh, material);
            Model3DGroup group = new Model3DGroup();
            group.Children.Add(model);
            return group;
        }

    }
}
