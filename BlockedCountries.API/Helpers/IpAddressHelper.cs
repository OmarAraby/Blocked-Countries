using System.Net;

namespace BlockedCountries.API.Helpers
{
    public static class IpAddressHelper
    {
        public static async Task<string> GetClientIpAddressAsync(HttpContext context)
        {
            //  Try X-Forwarded-For (reverse proxy, load balancer)
            if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwarded))
            {
                var firstIp = forwarded.ToString()
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault()?.Trim();

                if (!string.IsNullOrEmpty(firstIp) && IsValidPublicIpAddress(firstIp))
                    return firstIp;
            }

            // Try X-Real-IP (common header in nginx / proxies)
            if (context.Request.Headers.TryGetValue("X-Real-IP", out var realIp))
            {
                var ip = realIp.ToString().Trim();
                if (!string.IsNullOrEmpty(ip) && IsValidPublicIpAddress(ip))
                    return ip;
            }

            // Fall back to connection remote IP
            var remoteIp = context.Connection.RemoteIpAddress;
            if (remoteIp != null)
            {
                // Handle IPv4-mapped IPv6
                if (remoteIp.IsIPv4MappedToIPv6)
                    remoteIp = remoteIp.MapToIPv4();

                var ipString = remoteIp.ToString();

                // Handle IPv6 localhost (::1)
                if (ipString == "::1")
                    ipString = "127.0.0.1";

                if (IsValidPublicIpAddress(ipString))
                    return ipString;
            }

            //  Fallback: fetch the public IP of the current machine via external service
            return await GetPublicIpAsync();
        }

        public static bool IsValidIpAddress(string ip)
        {
            return IPAddress.TryParse(ip, out _);
        }

        public static bool IsPrivateIpAddress(string ip)
        {
            if (!IPAddress.TryParse(ip, out var address))
                return true; // Treat invalid as private for safety

            if (IPAddress.IsLoopback(address)) return true;

            // Only handle IPv4 ranges manually
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

        public static async Task<string> GetPublicIpAsync()
        {
            try
            {
                using var http = new HttpClient();
                var ip = await http.GetStringAsync("https://api.ipify.org");
                if (IsValidIpAddress(ip))
                    return ip.Trim();
            }
            catch
            {
                // ignore errors and fallback
            }

            return "127.0.0.1"; // fallback if all fails
        }
    }
}
