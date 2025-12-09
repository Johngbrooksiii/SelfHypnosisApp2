using Android.Media;
using HypnosisApp.Core.Services;

namespace HypnosisApp.UI.Platforms.Android.Services;

public class AndroidAudioService : IAudioPlaybackService
{
    private AudioTrack? _audioTrack;
    private bool _isPlaying;
    private float _masterVolume = 1.0f;

    public bool IsPlaying => _isPlaying;

    public async Task PlayPCMBufferAsync(byte[] pcmData, int sampleRate, CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>
        {
            try
            {
                // Stop any existing playback
                StopAudioTrack();

                // Calculate buffer size
                int bufferSize = AudioTrack.GetMinBufferSize(
                    sampleRate,
                    ChannelOut.Mono,
                    Encoding.Pcm16bit);

                // Create AudioTrack
                _audioTrack = new AudioTrack.Builder()
                    .SetAudioAttributes(new AudioAttributes.Builder()
                        .SetUsage(AudioUsageKind.Media)
                        .SetContentType(AudioContentType.Music)
                        .Build())
                    .SetAudioFormat(new AudioFormat.Builder()
                        .SetEncoding(Encoding.Pcm16bit)
                        .SetSampleRate(sampleRate)
                        .SetChannelMask(ChannelOut.Mono)
                        .Build())
                    .SetBufferSizeInBytes(bufferSize)
                    .SetTransferMode(AudioTrackMode.Stream)
                    .Build();

                if (_audioTrack == null)
                {
                    throw new InvalidOperationException("Failed to create AudioTrack");
                }

                // Set volume
                _audioTrack.SetVolume(_masterVolume);

                // Start playback
                _audioTrack.Play();
                _isPlaying = true;

                // Write data in chunks
                int offset = 0;
                int chunkSize = bufferSize;

                while (offset < pcmData.Length && !cancellationToken.IsCancellationRequested)
                {
                    int bytesToWrite = Math.Min(chunkSize, pcmData.Length - offset);
                    int bytesWritten = _audioTrack.Write(pcmData, offset, bytesToWrite);

                    if (bytesWritten < 0)
                    {
                        throw new InvalidOperationException($"Error writing audio data: {bytesWritten}");
                    }

                    offset += bytesWritten;
                }

                // Wait for playback to complete
                if (!cancellationToken.IsCancellationRequested)
                {
                    _audioTrack.Stop();
                }

                _isPlaying = false;
            }
            catch (Exception ex)
            {
                _isPlaying = false;
                System.Diagnostics.Debug.WriteLine($"Audio playback error: {ex.Message}");
                throw;
            }
        }, cancellationToken);
    }

    public Task StopAllAudioAsync()
    {
        StopAudioTrack();
        return Task.CompletedTask;
    }

    public void SetMasterVolume(float volume)
    {
        _masterVolume = Math.Clamp(volume, 0.0f, 1.0f);
        _audioTrack?.SetVolume(_masterVolume);
    }

    private void StopAudioTrack()
    {
        if (_audioTrack != null)
        {
            try
            {
                if (_audioTrack.PlayState == PlayState.Playing)
                {
                    _audioTrack.Stop();
                }
                _audioTrack.Release();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error stopping audio track: {ex.Message}");
            }
            finally
            {
                _audioTrack = null;
                _isPlaying = false;
            }
        }
    }
}
