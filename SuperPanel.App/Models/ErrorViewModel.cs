using System;

namespace SuperPanel.App.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public int StatusCode { get; set; }
        public string Error { get; set; }
        public string Title { get; set; }
    }
}
