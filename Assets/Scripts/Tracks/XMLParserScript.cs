using UnityEngine;
using System.Collections;
using System.Xml;
using System;
using System.Globalization;

namespace XMLParser
{
    public record Point
    {
        public float x; public float y; public float z; public float t;
    }

    public record Track
    {
        public int pdgCode;
        public int parentID;
        public Point[] points;
    }

    public record Event
    {
        public Track[] tracks;
    }

    public record DataChain
    {
        public Event[] events;
    }

    public static class XMLParserScript
    {
        public static DataChain ParseXMLData(string filePath = "Assets/TrackData/trackdata.xml")
        {
            DataChain dataChain = new DataChain();
            XmlDocument data = new XmlDocument();
            data.Load(filePath);

            XmlElement? xDataChain = data.DocumentElement;
            if (xDataChain != null)
            {
                int nEvents = int.Parse(xDataChain.Attributes.GetNamedItem("nEvents").Value, CultureInfo.InvariantCulture);
                dataChain.events = new Event[nEvents];

                //Debug.Log($"Made new DataChain, number of events: {nEvents}");

                foreach (XmlElement xEvent in xDataChain)
                {

                    int eventID = int.Parse(xEvent.Attributes.GetNamedItem("id").Value, CultureInfo.InvariantCulture);
                    int nTracks = int.Parse(xEvent.Attributes.GetNamedItem("nTracks").Value, CultureInfo.InvariantCulture);

                    dataChain.events[eventID] = new Event();
                    dataChain.events[eventID].tracks = new Track[nTracks];
                    //Debug.Log($"Event {eventID}, tracks: {nTracks}");



                    foreach (XmlNode xTrack in xEvent.ChildNodes)
                    {
                        int trackID = int.Parse(xTrack.Attributes.GetNamedItem("id").Value, CultureInfo.InvariantCulture);
                        int nPoints = int.Parse(xTrack.Attributes.GetNamedItem("nPoints").Value, CultureInfo.InvariantCulture);
                        int parentID = int.Parse(xTrack.Attributes.GetNamedItem("parentID").Value, CultureInfo.InvariantCulture);
                        int PDGcode = int.Parse(xTrack.Attributes.GetNamedItem("PDGcode").Value, CultureInfo.InvariantCulture);

                        dataChain.events[eventID].tracks[trackID] = new Track();
                        dataChain.events[eventID].tracks[trackID].points = new Point[nPoints];
                        dataChain.events[eventID].tracks[trackID].parentID = parentID;
                        dataChain.events[eventID].tracks[trackID].pdgCode = PDGcode;

                        //Debug.Log($"Track {trackID}, points: {nPoints}, PDGcode: {PDGcode}");


                        foreach (XmlNode xPoint in xTrack.ChildNodes)
                        {
                            int pointID = int.Parse(xPoint.Attributes.GetNamedItem("id").Value, CultureInfo.InvariantCulture);
                            float x = Convert.ToSingle(xPoint.Attributes.GetNamedItem("x").Value, CultureInfo.InvariantCulture);
                            float y = Convert.ToSingle(xPoint.Attributes.GetNamedItem("y").Value, CultureInfo.InvariantCulture);
                            float z = Convert.ToSingle(xPoint.Attributes.GetNamedItem("z").Value, CultureInfo.InvariantCulture);
                            float t = Convert.ToSingle(xPoint.Attributes.GetNamedItem("t").Value, CultureInfo.InvariantCulture);

                            dataChain.events[eventID].tracks[trackID].points[pointID] = new Point();
                            dataChain.events[eventID].tracks[trackID].points[pointID].x = x;
                            dataChain.events[eventID].tracks[trackID].points[pointID].y = y;
                            dataChain.events[eventID].tracks[trackID].points[pointID].z = z;
                            dataChain.events[eventID].tracks[trackID].points[pointID].t = t;

                        }
                    }

                }

                //Debug.Log("Finished parsing XML file");
            }

            return dataChain;
        }
    }

}
