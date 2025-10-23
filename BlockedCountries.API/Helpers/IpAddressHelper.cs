namespace BlockedCountries.API.Helpers
{
    public static class IpAddressHelper
    {
        public static string GetClientIpAddress(HttpContext context)
        {
            // Try X-Forwarded-For (for reverse proxies/Load balancers)
            if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwarded))
            {
                var firstIp = forwarded.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault()?.Trim();

                if (!string.IsNullOrEmpty(firstIp) && IsValidPublicIpAddress(firstIp))
                    return firstIp;
            }

            // Try X-Real-IP
            if (context.Request.Headers.TryGetValue("X-Real-IP", out var realIp))
            {
                var ip = realIp.ToString().Trim();
                if (!string.IsNullOrEmpty(ip) && IsValidPublicIpAddress(ip))
                    return ip;
            }

            // Fall back to RemoteIpAddress
            var remoteIp = context.Connection.RemoteIpAddress;
            if (remoteIp != null)
            {
                // Handle IPv4-mapped IPv6 addresses and convert to IPv4 if possible
                if (remoteIp.IsIPv4MappedToIPv6)
                    remoteIp = remoteIp.MapToIPv4();

                var ipString = remoteIp.ToString();

                // Convert IPv6 localhost to IPv4
                if (ipString == "::1")
                    return "127.0.0.1";

                if (IsValidPublicIpAddress(ipString))
                    return ipString;
            }

            return "127.0.0.1"; // fallback for localhost
        }

        public static bool IsValidIpAddress(string ip)
        {
            return System.Net.IPAddress.TryParse(ip, out _);
        }

        public static bool IsPrivateIpAddress(string ip)
        {
            if (!System.Net.IPAddress.TryParse(ip, out var address))
                return true; // Treat invalid as private

            // Handle IPv6 localhost
            if (address.Equals(System.Net.IPAddress.IPv6Loopback))
                return true;

            // Handle IPv4 localhost
            if (System.Net.IPAddress.IsLoopback(address))
                return true;

            // Check for private IP ranges (IPv4)
            if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                var bytes = address.GetAddressBytes();

                // 10.0.0.0/8
                if (bytes[0] == 10) return true;

                // 172.16.0.0/12
                if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31) return true;

                // 192.168.0.0/16
                if (bytes[0] == 192 && bytes[1] == 168) return true;

                // 127.0.0.0/8
                if (bytes[0] == 127) return true;
            }

            return false;
        }

        public static bool IsValidPublicIpAddress(string ip)
        {
            return IsValidIpAddress(ip) && !IsPrivateIpAddress(ip);
        }
    }
}