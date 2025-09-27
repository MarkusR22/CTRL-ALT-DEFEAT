using System;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;
using Robocode.TankRoyale.BotApi.Graphics;


// ------------------------------------------------------------------
// MyFirstBot
// ------------------------------------------------------------------
// A sample bot original made for Robocode by Mathew Nelson.
//
// Probably the first bot you will learn about.
// Moves in a seesaw motion and spins the gun around at each end.
// ------------------------------------------------------------------
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
        GunTurnRate = 15;

        BodyColor = Color.Red;
        TurretColor = Color.Black;
        RadarColor = Color.Yellow;
        BulletColor = Color.Green;
        ScanColor = Color.Green;

        // Repeat while the bot is running
        while (IsRunning)
        {
            Forward(100);
            TurnGunLeft(360);
            Back(100);
            TurnGunLeft(360);
            TurnGunRight(10);
        }
    }

    // We saw another bot -> fire!
    public override void OnScannedBot(ScannedBotEvent evt)
    {

        var bearingFromGun = GunBearingTo(evt.X, evt.Y);

        // Turn the gun toward the scanned bot
        TurnGunLeft(bearingFromGun);

        // If it is close enough, fire!
        if (Math.Abs(bearingFromGun) <= 2 && GunHeat == 0)
            Fire(2);

        // Generates another scan event if we see a bot.
        // We only need to call this if the gun (and therefore radar)
        // are not turning. Otherwise, scan is called automatically.
        if (bearingFromGun == 0)
            Rescan();
    }

    // We were hit by a bullet -> turn perpendicular to the bullet
    public override void OnHitByBullet(HitByBulletEvent evt)
    {
        // Calculate the bearing to the direction of the bullet
        var bearing = CalcBearing(evt.Bullet.Direction);
        

        // Turn 90 degrees to the bullet direction based on the bearing
        TurnRight(90 - bearing);

    }
    public void HitWallEvent(int turnNumber)
    {
        TurnRight(120);
        Forward(100);
    }
    public override void OnHitBot(HitBotEvent e)
    {
        // Turn gun to the bullet direction
        var direction = DirectionTo(e.X, e.Y);
        var gunBearing = NormalizeRelativeAngle(direction - GunDirection);
        TurnGunRight(gunBearing);

        // Fire hard
        Fire(3);
    }
}