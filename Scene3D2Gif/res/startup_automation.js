class StartupConfiguration
{
    function getStartup3DFile(app)
    {
        var bLoadBig = true;
        var scene3DObject2Load = bLoadBig ? "res/leo/leo.obj" : "res/sw/falcon.3ds";
        app.OnInsertAgain(scene3DObject2Load);
        return scene3DObject2Load;
    }
}
