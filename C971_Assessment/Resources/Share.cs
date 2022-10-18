using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace C971_Assessment.Resources
{
    internal class Share
    {
        public static async Task ShareText(string title, string notes)
        {
            await Xamarin.Essentials.Share.RequestAsync(new ShareTextRequest
            {
                Text = title + ": " + notes,
                Title = "Course Details"
            });
            // (Montemagno et al., 2022)
        }
    }
}