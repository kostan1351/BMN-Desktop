using UnityEngine;
namespace XMLParser
{
    public class CubeScript : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        public GameObject linePrefab;
        
        void Start()
        {
            DataChain data = XMLParserScript.ParseXMLData();
            Track[] tracks = data.events[0].tracks;

            
            
            

            foreach (Track track in tracks)
            {
                GameObject line = Instantiate(linePrefab);
                lineRenderer = line.GetComponent<LineRenderer>();
                int vertexCount = track.points.Length;
                Vector3[] vertices = new Vector3[vertexCount];
                int i = 0;

                foreach (Point point in track.points)
                {
                    
                    vertices[i] = new Vector3(point.x, point.y, point.z);
                    i++;
                    
                }
                DrawPoly(vertices, vertexCount);

            }

        }

        void DrawPoly(Vector3[] vertexPositions, int vertices)
        {
            lineRenderer.loop = false;
            lineRenderer.positionCount = vertices;
            lineRenderer.SetPositions(vertexPositions);
        }

        
        void Update()
        {

        }
    }
}