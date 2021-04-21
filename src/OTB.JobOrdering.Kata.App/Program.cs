using OTB.JobOrdering.Kata.Application.Services;
using System;

namespace OTB.JobOrdering.Kata.App
{
    class Program
    {
        private static IJobOrderService _jobOrderService;
        static void Main(string[] args)
        {
            _jobOrderService = new JobOrderService();

            Console.WriteLine("Welcome to Job Processing System");

            Console.WriteLine("Please enter jobs you want to process. \n As mentioned in the readme file. (e.g. a => or 'a =>' or 'a =>\n b =>\n c =>' or 'a =>\n b => c\n c => f\n d => a\n e => b\n f =>'");
            Console.WriteLine("When done please press ESC to Stop and get Results.");

            string jobNames = Console.ReadLine();

            var replicaJobs = jobNames.Replace("\\n", "\n");


            var result = _jobOrderService.OrderJobs(replicaJobs);
            Console.WriteLine($"Job Sequence is: {result}");


        }
    }
}
