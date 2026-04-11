using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace sectograph
{
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private BasicEffect _basicEffect;

    private List<CalendarEvent> _myEvents;
    private Vector2 _clockCenter;
    private float _clockRadius;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        // Targetkan 30 FPS saja karena ini cuma jam (Hemat CPU/GPU)
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d);
    }

protected override void Initialize()
    {
        _clockCenter = new Vector2(
            _graphics.PreferredBackBufferWidth / 2f, 
            _graphics.PreferredBackBufferHeight / 2f);
        _clockRadius = 200f;

        // Kamera dengan depth -1 sampai 1 agar tidak blank/gelap
        _basicEffect = new BasicEffect(GraphicsDevice)
        {
            VertexColorEnabled = true,
            Projection = Matrix.CreateOrthographicOffCenter(
                0, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height, 0,
                -1, 1) 
        };

        // Data dummy
        DateTime today = DateTime.Today;
        _myEvents = new List<CalendarEvent>
        {
            new CalendarEvent("Ngoding", today.AddHours(9), today.AddHours(11.5), Color.DarkTurquoise),
            new CalendarEvent("Makan Siang", today.AddHours(12), today.AddHours(13), Color.OrangeRed),
            new CalendarEvent("Rapat", today.AddHours(14), today.AddHours(15.75), Color.MediumPurple)
        };

        base.Initialize();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.FromNonPremultiplied(30, 30, 30, 255));

        // Matikan Culling agar segitiga tetap digambar
        RasterizerState rasterizerState = new RasterizerState { CullMode = CullMode.None }; 
        GraphicsDevice.RasterizerState = rasterizerState;

        foreach (var pass in _basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();

            // 1. Gambar irisan acara (Wedges)
            foreach (var ev in _myEvents)
            {
                var vertices = ClockLogic.CreateWedge(_clockCenter, _clockRadius, ev.StartTime, ev.EndTime, ev.EventColor);
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length / 3);
            }

            // 2. Gambar penanda angka jam (12 Garis tepi)
            var tickVertices = ClockLogic.CreateClockTicks(_clockCenter, _clockRadius, Color.LightGray);
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, tickVertices, 0, 12);

            // 3. Gambar jarum waktu saat ini (Merah)
            var needleVertices = ClockLogic.CreateCurrentTimeNeedle(_clockCenter, _clockRadius, DateTime.Now, Color.Red);
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, needleVertices, 0, 1);
        }

        base.Draw(gameTime);
    }
}
}