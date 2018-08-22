class StartupConfiguration
{
    function getStartup3DFile(app)
    {
        var bLoadBig = true;
        var scene3DObject2Load = bLoadBig ? "res/leo/leo.obj" : "res/sw/falcon.3ds";
        scene3DObject2Load = "res/DinoRider.3ds";
        app.OnInsertAgain(scene3DObject2Load);

        //app.rezip("d:/temp/rezipper/in", "d:/temp/rezipper/temp", "d:/System Volume Information/SPP/a9/v/vvv.r");

        return scene3DObject2Load;
    }
}
