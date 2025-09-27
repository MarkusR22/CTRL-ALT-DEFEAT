using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;
using Robocode.TankRoyale.BotApi.Graphics;
using System;

public class Geoff : Bot
{
    // The main method starts our bot
    static void Main(string[] args)
    {
        new Geoff().Start();
    }

    // Called when a new round is started -> initialize and do some movement
    public override void Run()
    {
        // Set colors
        BodyColor = Color.FromArgb(0, 128, 255); // Blue
        GunColor = Color.FromArgb(255, 0, 0); // Red
        RadarColor = Color.FromArgb(255, 255, 0); // Yellow
        BulletColor = Color.FromArgb(255, 255, 255); // White
        ScanColor = Color.FromArgb(0, 255, 0); // Green

        // Move ahead a bit
        Ahead(100);

        // Turn to face the center of the arena
        var centerX = ArenaWidth / 2;
        var centerY = ArenaHeight / 2;
        var bearingToCenter = CalcBearing(centerX, centerY);
        TurnRight(bearingToCenter);

        StopAndGO();
        // Move to the center (or beyond)
        Ahead(500);

        // Turn gun to turn infinitely right
        TurnGunRight(double.MaxValue);

    }

    // We saw another bot -> fire!
    public override void OnScannedBot(ScannedBotEvent evt)
    {
        var distance = DistanceTo(e.X, e.Y);

        // Should we stop, or just fire?
        if (stopWhenSeeEnemy)
        {
            // Stop movement
            Stop();
            // Call our custom firing method
            SmartFire(distance);
            // Rescan for another bot
            Rescan();
            // This line will not be reached when scanning another bot.
            // So we did not scan another bot -> resume movement
            Resume();
        }
        else
            SmartFire(distance);
    }
    public void shoot(double distance)
    {
        if (distance > 200 || Energy < 15)
            Fire(1);
        else if (distance > 50)
            Fire(2);
        else
            Fire(3);
    }

    // We were hit by a bullet -> turn perpendicular to the bullet
    public override void OnHitByBullet(HitByBulletEvent evt)
    {
        // Calculate the bearing to the direction of the bullet
        var bearing = CalcBearing(evt.Bullet.Direction);

        // Turn perpendicular to the bullet
        if (!hasTunred)
        {
            TurnRight(bearing + 90);
            hasTunred = true;
        }
        else
        {
            TurnLeft(bearing + 90);
            hasTunred = false;
        }
    }
    public void StopAndGO()
    {
        while (IsRunning)
        {
            var centerX = ArenaWidth / 2;
            var centerY = ArenaHeight / 2;
            var bearingToCenter = CalcBearing(centerX, centerY);
            TurnRight(bearingToCenter);
            Ahead(500);
            Random rand = new Random();
            int r = rand.Next(0, 3);
            if (r == 0)
                TurnRight(90);
            else if (r == 1)
                TurnLeft(90);
            else
                TurnRight(180);
            Ahead(500);
        }  
    }
}