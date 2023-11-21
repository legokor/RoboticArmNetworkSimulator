using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

using System.Web;
public class NetworkController : MonoBehaviour
{
    public HttpListener listener;
    public string url = "http://localhost:8000/";

    Thread SocketThread;

    [SerializeField] private InputController ic;

    private CylindricalCoordinate lastPos = new CylindricalCoordinate();

    public CylindricalCoordinate QueryLastPos(){
        return lastPos;
    }

    void Start()
    {
        Application.runInBackground = true;

        startServer();
    }

    private void Update(){
        
        //weird ranges because of the weird setup of the sliders

        //I set it up so that the hands 0.5m from the center corresponds
        //to the largest possible deflection of the robot arms

        ic.SetHorizontalExtension(((lastPos.r) * 38*3)-24.0f); //mapping: -24 to 14 
        ic.SetRotationAngle(((lastPos.f)/6.28f)+0.5f); //mapping: 0 to 1
        ic.SetVerticalElevation(((lastPos.z - 0.25f) * 80)-35); //mapping: -35 to 5
    }

    void startServer()
    {
        listener = new HttpListener();
        listener.Prefixes.Add(url);
        listener.Start();
        UnityEngine.Debug.Log("Listening for connections on " + url);

        SocketThread = new Thread(networkManager);
        SocketThread.IsBackground = true;
        SocketThread.Start();
    }

    private void networkManager()
    {
        // Handle requests
        Task listenTask = HandleIncomingConnections();
        listenTask.GetAwaiter().GetResult();

        // Close the listener
        listener.Close();
    }

    public async Task HandleIncomingConnections()
    {
        bool runServer = true;

        while (runServer)
        {
            HttpListenerContext ctx = await listener.GetContextAsync();

            // Peel out the requests and response objects
            HttpListenerRequest req = ctx.Request;
            HttpListenerResponse resp = ctx.Response;

            // Print out some info about the request
            UnityEngine.Debug.Log(req.Url.ToString());
            //UnityEngine.Debug.Log(req.HttpMethod);
            //UnityEngine.Debug.Log(req.UserHostName);
            //UnityEngine.Debug.Log(req.UserAgent);
            
            Uri uri = new Uri(req.Url.ToString());
            NameValueCollection queryParameters = HttpUtility.ParseQueryString(uri.Query);

            //string r = queryParameters["r"];
            //string f = queryParameters["f"];
            //string z = queryParameters["z"];

            //UnityEngine.Debug.Log(r + " " +f+ " " + z);
            //ic.SetHorizontalExtension();
            //ic.SetRotationAngle(float.Parse(f));
            //ic.SetVerticalElevation(float.Parse(z));

            lastPos.f = float.Parse(queryParameters["f"]);
            lastPos.r = float.Parse(queryParameters["r"]);
            lastPos.z = float.Parse(queryParameters["z"]);
            

            resp.Close();

            //maybe better to increase it or couple it to framerate 
            Thread.Sleep(10);
        }
    }

    public class CylindricalCoordinate {
        public float r { get; set; }
        public float f { get; set; }
        public float z { get; set; }
    }
}


