using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkController : MonoBehaviour
{
    public HttpListener listener;
    public string url = "http://localhost:8000/";

    Thread SocketThread;

    void Start()
    {
        Application.runInBackground = true;

        startServer();
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
            UnityEngine.Debug.Log(req.HttpMethod);
            UnityEngine.Debug.Log(req.UserHostName);
            UnityEngine.Debug.Log(req.UserAgent);

            resp.Close();

            Thread.Sleep(10);
        }
    }
}
