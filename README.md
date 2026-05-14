AI Usage for Part 2:

For my first commit, I intially used AI as a learning guide to fix my previous code based on the comments left on g-sheet. Then, I prompted questions to learn about the use of branches, its difference to main, and how to properly pull requests. Initially, I created a Part 2 branch and immediately opened a pull request without completing the required 3 commits first, causing an unintentional merge to main, a Git workflow mistake I'll correct in future commits by finishing all 3 commits per part before PR. 

For the next commits, I built receipt printing with auto-numbering, current date/time, and cart totals. I asked AI: "In C#, how do I create a receipt number that increases by 1 each time and saves for the session?" AI answered: "Use int receiptNum = int.Parse(HttpContext.Session.GetString("receiptNum") ?? "0"); receiptNum++; HttpContext.Session.SetString("receiptNum", receiptNum.ToString()); to store it in session storage." I also asked: "What's a simple way in C# to display today's date and time on a receipt?" It suggested: "Try DateTime.Now.ToString("MMM dd, yyyy, hh:mm tt") for a clear format." I handled all testing, cart limits, and empty cart cases on my own.

For the last commit, I prompted: "How do I display the full receipt history list on a new page?" AI suggested: "Loop through the list with @foreach(var receipt in ViewBag.History). 
