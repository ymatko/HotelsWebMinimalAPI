namespace HotelsWebMinimalAPI.Data
{
    public class HotelRepository : IHotelRepository
    {
        private readonly HotelDb _hotelDb;
        public HotelRepository(HotelDb hotelDb)
        {
            _hotelDb = hotelDb;
        }
        public Task<List<Hotel>> GetHotelsAsync() => 
            _hotelDb.Hotels.ToListAsync();
        public async Task<Hotel> GetHotelAsync(int hotelId) =>
            await _hotelDb.Hotels.FindAsync(new object[] { hotelId });
        public async Task InsertHotelAsync(Hotel hotel) =>
            await _hotelDb.Hotels.AddAsync(hotel);
        public async Task UpdateHotelAsync(Hotel hotel)
        {
            var hotelFromDb = await _hotelDb.Hotels.FindAsync(new object[] { hotel.Id });
            if (hotelFromDb == null) return;
            hotelFromDb.Latitude = hotel.Latitude;
            hotelFromDb.Longitude = hotel.Longitude;
            hotelFromDb.Name = hotel.Name;
        }
        public async Task DeleteHotelAsync(int hotelId)
        {
            var hotelFromDb = await _hotelDb.Hotels.FindAsync(new object[] { hotelId });
            if (hotelFromDb == null) return;
            _hotelDb.Hotels.Remove(hotelFromDb);
        }
        public async Task SaveAsync() =>
            await _hotelDb.SaveChangesAsync();

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _hotelDb.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
