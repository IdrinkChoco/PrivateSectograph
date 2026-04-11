using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public static class ClockLogic
{
    // Mengubah waktu menjadi sudut Radian (Format 12 Jam)
    public static float TimeToRadians(DateTime time)
    {
        // Ambil jam (format 12 jam) dan menit
        float hours = time.Hour % 12;
        float minutes = time.Minute;

        // Total menit dari jam 12:00
        float totalMinutes = (hours * 60f) + minutes;

        // Proporsi terhadap total menit dalam 12 jam (720 menit)
        float proportion = totalMinutes / 720f;

        // Kalikan dengan 2 PI (360 derajat)
        // Hasilnya: 0 rad di jam 12, PI/2 di jam 3, PI di jam 6.
        return proportion * MathHelper.TwoPi;
    }

    // Membuat mesh (kumpulan segitiga) untuk satu event/irisan
    public static VertexPositionColor[] CreateWedge(Vector2 center, float radius, DateTime start, DateTime end, Color color)
    {
        float startAngle = TimeToRadians(start);
        float endAngle = TimeToRadians(end);
        
        // Jika event melewati jam 12 (misal 11:00 ke 01:00)
        if (endAngle < startAngle) endAngle += MathHelper.TwoPi;

        // Tentukan kehalusan lengkungan (semakin banyak, semakin bulat)
        int segments = 30; 
        float angleStep = (endAngle - startAngle) / segments;

        // Jumlah vertex = (segments * 3) karena 1 segitiga butuh 3 titik
        VertexPositionColor[] vertices = new VertexPositionColor[segments * 3];
        int vertexIndex = 0;

        for (int i = 0; i < segments; i++)
        {
            float currentAngle = startAngle + (i * angleStep);
            float nextAngle = currentAngle + angleStep;

            // Titik 1: Pusat jam
            vertices[vertexIndex++] = new VertexPositionColor(new Vector3(center, 0), color);

            // Titik 2: Tepi lingkaran (current angle)
            // Rumus posisi jarum jam: X = sin(angle), Y = -cos(angle)
            float x1 = center.X + (float)Math.Sin(currentAngle) * radius;
            float y1 = center.Y - (float)Math.Cos(currentAngle) * radius;
            vertices[vertexIndex++] = new VertexPositionColor(new Vector3(x1, y1, 0), color);

            // Titik 3: Tepi lingkaran (next angle)
            float x2 = center.X + (float)Math.Sin(nextAngle) * radius;
            float y2 = center.Y - (float)Math.Cos(nextAngle) * radius;
            vertices[vertexIndex++] = new VertexPositionColor(new Vector3(x2, y2, 0), color);
        }

        return vertices;
    }

    // Step 2 disini
    // Membuat garis penanda jam (12 garis di tepi lingkaran)
    public static VertexPositionColor[] CreateClockTicks(Vector2 center, float radius, Color color)
    {
        VertexPositionColor[] vertices = new VertexPositionColor[24]; // 12 garis x 2 titik = 24
        int index = 0;
        
        for (int i = 0; i < 12; i++)
        {
            // Menghitung sudut tiap jam (1 jam = 30 derajat atau PI/6 radian)
            float angle = i * (MathHelper.TwoPi / 12f);
            
            // Garis dimulai dari dalam sedikit (90% dari radius) ke tepi luar
            float innerRadius = radius * 0.9f; 
            
            float x1 = center.X + (float)Math.Sin(angle) * innerRadius;
            float y1 = center.Y - (float)Math.Cos(angle) * innerRadius;
            
            float x2 = center.X + (float)Math.Sin(angle) * radius;
            float y2 = center.Y - (float)Math.Cos(angle) * radius;
            
            vertices[index++] = new VertexPositionColor(new Vector3(x1, y1, 0), color);
            vertices[index++] = new VertexPositionColor(new Vector3(x2, y2, 0), color);
        }
        return vertices;
    }

    // Membuat jarum yang menunjukkan waktu detik ini juga
    public static VertexPositionColor[] CreateCurrentTimeNeedle(Vector2 center, float radius, DateTime currentTime, Color color)
    {
        float angle = TimeToRadians(currentTime);
        
        float x = center.X + (float)Math.Sin(angle) * radius;
        float y = center.Y - (float)Math.Cos(angle) * radius;
        
        VertexPositionColor[] vertices = new VertexPositionColor[2];
        vertices[0] = new VertexPositionColor(new Vector3(center, 0), color); // Titik pusat
        vertices[1] = new VertexPositionColor(new Vector3(x, y, 0), color);    // Titik tepi
        
        return vertices;
    }
}