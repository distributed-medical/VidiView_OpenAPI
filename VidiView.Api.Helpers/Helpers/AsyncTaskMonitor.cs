using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using VidiView.Api.DataModel;

namespace VidiView.Api.Helpers;

/// <summary>
/// This helper class is used to wait for a task to 
/// complete on the server, while continuously reporting
/// progress through the <see cref="CurrentStatus"/> property
/// </summary>
public class AsyncTaskMonitor : INotifyPropertyChanged
{
    /// <summary>
    /// Raised when property is changed
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    private AsyncTaskStatus? _currentStatus;
    private TimeSpan _pollingInterval = TimeSpan.FromMilliseconds(250);

    public AsyncTaskMonitor()
    {
    }

    /// <summary>
    /// Interval to wait between status polling calls
    /// </summary>
    public TimeSpan PollingInterval
    {
        get => _pollingInterval;
        set
        {
            _pollingInterval = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PollingInterval)));
        }
    }

    /// <summary>
    /// Current task status. This gets updated after each poll
    /// </summary>
    public AsyncTaskStatus? CurrentStatus
    {
        get => _currentStatus;
        private set
        {
            _currentStatus = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentStatus)));
        }
    }

    /// <summary>
    /// Wait for a task to finish. This will continuously poll the server
    /// for updated status information and update the <see cref="CurrentStatus"/> property.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="status">The task information to await</param>
    /// <returns>The final status of this task</returns>
    public async Task<AsyncTaskStatus> WaitForCompletionAsync(HttpClient client, AsyncTaskStatus status)
    {
        ArgumentNullException.ThrowIfNull(client, nameof(client));
        CurrentStatus = status ?? throw new ArgumentNullException(nameof(status));

        return await Task.Run(async () =>
        {
            while (CurrentStatus.State == TaskState.WaitingForExecution 
                   || CurrentStatus.State == TaskState.Running)
            {
                var link = status.Links.GetRequired(Rel.Self).AsTemplatedLink();

                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = (Uri)link,
                    Content = HttpContentFactory.CreateBody(null)
                };

                // Always wait a while before querying status
                await Task.Delay(PollingInterval).ConfigureAwait(false);
                var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

                if (response.StatusCode == HttpStatusCode.SeeOther)
                {
                    // Completed
                    CurrentStatus = await response.DeserializeAsync<AsyncTaskStatus>()
                    with
                    {
                        // It is expected that the status is Success when we get a redirect,
                        // but to be certain the loop exits, we explicitly sets the state here too
                        State = TaskState.SuccessfullyCompleted
                    };

                }
                else if (response.IsSuccessStatusCode)
                {
                    // Updated status
                    CurrentStatus = await response.DeserializeAsync<AsyncTaskStatus>();

                    // If the status indicates a failure, it will exit the loop
                }
                else
                {
                    // Some kind of error
                    await response.AssertSuccessAsync().ConfigureAwait(false);
                }
            }

            // Exit and return status
            return CurrentStatus;
        });
    }

#if WINRT
    /// <summary>
    /// Wait for a task to finish. This will continuously poll the server
    /// for updated status information and update the <see cref="CurrentStatus"/> property.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="status">The task information to await</param>
    /// <returns>The final status of this task</returns>
    public async Task<AsyncTaskStatus> WaitForCompletionAsync(Windows.Web.Http.HttpClient client, AsyncTaskStatus status)
    {
        ArgumentNullException.ThrowIfNull(client, nameof(client));
        CurrentStatus = status ?? throw new ArgumentNullException(nameof(status));

        return await Task.Run(async () =>
        {
            while (CurrentStatus.State == TaskState.WaitingForExecution 
                   || CurrentStatus.State == TaskState.Running)
            {
                var link = status.Links.GetRequired(Rel.Self).AsTemplatedLink();

                var request = new Windows.Web.Http.HttpRequestMessage()
                {
                    Method = Windows.Web.Http.HttpMethod.Get,
                    RequestUri = (Uri)link,
                    Content = HttpContentFactoryWinRT.CreateBody(null)
                };

                // Always wait a while before querying status
                await Task.Delay(PollingInterval).ConfigureAwait(false);
                var response = await client.SendRequestAsync(request, Windows.Web.Http.HttpCompletionOption.ResponseHeadersRead);

                if (response.StatusCode == Windows.Web.Http.HttpStatusCode.SeeOther)
                {
                    // Completed
                    CurrentStatus = await response.DeserializeAsync<AsyncTaskStatus>()
                    with
                    {
                        // It is expected that the status is Success when we get a redirect,
                        // but to be certain the loop exits, we explicitly sets the state here too
                        State = TaskState.SuccessfullyCompleted
                    };
                }
                else if (response.IsSuccessStatusCode)
                {
                    // Updated status
                    CurrentStatus = await response.DeserializeAsync<AsyncTaskStatus>();

                    // If the status indicates a failure, it will exit the loop
                }
                else
                {
                    // Some kind of error
                    await response.AssertSuccessAsync().ConfigureAwait(false);
                }
            }

            // Exit and return status
            return CurrentStatus;
        });
    }
#endif
}
