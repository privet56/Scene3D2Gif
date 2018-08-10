class StartupConfiguration
{
    function getStartup3DFile(app)
    {
        var scene3DObject2Load = "res/sw/falcon.3ds";
        app.OnInsertAgain(scene3DObject2Load);
        return scene3DObject2Load;
    }
}

/*
package Startup
{
    class StartupConfiguration
    {
        function getStartup3DFile(launchArguements)
        {
            return "res/lea/lea.obj";
        }
    }
}
*/
