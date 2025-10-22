using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaPlatform.Domain.Entities
{
    // CS0122 hatasını çözmek için public yaptık
    // Rota, koordinat çiftlerini tutar ve gezilebilir (IEnumerable) olmalıdır.
    public class Route : IEnumerable<(int X, int Y)>
    {
        private readonly List<(int X, int Y)> _stops = new(); // (X, Y) koordinatları

        // Koleksiyon Başlatıcıları desteklemek için gereklidir.
        public void Add(int x, int y)
        {
            _stops.Add((x, y));
        }

        // CS0122 hatasını çözmek için kurucuyu public yaptık
        public Route() { }

        // IEnumerable arayüzünün uygulanması.
        public IEnumerator<(int X, int Y)> GetEnumerator()
        {
            return _stops.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}