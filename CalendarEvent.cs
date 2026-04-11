using Microsoft.Xna.Framework;
using System;

public class CalendarEvent
{
    public string Title { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Color EventColor { get; set; }

    public CalendarEvent(string title, DateTime start, DateTime end, Color color)
    {
        Title = title;
        StartTime = start;
        EndTime = end;
        EventColor = color;
    }
}