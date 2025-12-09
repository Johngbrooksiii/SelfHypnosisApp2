using Android.Speech.Tts;
using HypnosisApp.Core.Services;
using Java.Util;

namespace HypnosisApp.UI.Platforms.Android.Services;

public class AndroidNarrationEngine : Java.Lang.Object, INarrationEngine, TextToSpeech.IOnInitListener
{
    private TextToSpeech? _textToSpeech;
    private bool _isInitialized;
    private readonly TaskCompletionSource<bool> _initializationTcs = new();

    public AndroidNarrationEngine()
    {
        var context = Platform.CurrentActivity ?? throw new InvalidOperationException("Current activity is null");
        _textToSpeech = new TextToSpeech(context, this);
    }

    public void OnInit(OperationResult status)
    {
        if (status == OperationResult.Success)
        {
            _textToSpeech?.SetLanguage(Locale.Us);
            _isInitialized = true;
            _initializationTcs.TrySetResult(true);
        }
        else
        {
            _isInitialized = false;
            _initializationTcs.TrySetResult(false);
        }
    }

    public async Task SpeakAsync(string text, float speed = 0.8f)
    {
        if (!_isInitialized)
        {
            // Wait for initialization
            await _initializationTcs.Task;
        }

        if (_textToSpeech == null || !_isInitialized)
        {
            throw new InvalidOperationException("TextToSpeech is not initialized");
        }

        // Set speech rate (0.8 = 80% of normal speed, good for relaxation)
        _textToSpeech.SetSpeechRate(speed);

        // Create a TaskCompletionSource to wait for speech to complete
        var tcs = new TaskCompletionSource<bool>();

        // Speak the text
        var utteranceId = Guid.NewGuid().ToString();
        _textToSpeech.Speak(text, QueueMode.Flush, null, utteranceId);

        // Set up completion callback
        _textToSpeech.SetOnUtteranceProgressListener(new UtteranceProgressListener(tcs));

        // Wait for speech to complete
        await tcs.Task;
    }

    public void Stop()
    {
        _textToSpeech?.Stop();
    }

    private class UtteranceProgressListener : UtteranceProgressListener
    {
        private readonly TaskCompletionSource<bool> _tcs;

        public UtteranceProgressListener(TaskCompletionSource<bool> tcs)
        {
            _tcs = tcs;
        }

        public override void OnDone(string? utteranceId)
        {
            _tcs.TrySetResult(true);
        }

        public override void OnError(string? utteranceId)
        {
            _tcs.TrySetException(new Exception("TTS error occurred"));
        }

        public override void OnStart(string? utteranceId)
        {
            // Speech started
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _textToSpeech?.Stop();
            _textToSpeech?.Shutdown();
            _textToSpeech?.Dispose();
        }
        base.Dispose(disposing);
    }
}
