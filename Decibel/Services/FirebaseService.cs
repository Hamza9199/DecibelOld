using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Storage;
using DotNetEnv;

namespace Decibel.Services
{
        public class FirebaseService
        {
                private static string ApiKey;
                private static string Bucket;
                private static string AuthEmail;
                private static string AuthPassword;
                private static string AuthDomain;

                public FirebaseService()
                {
                        Env.Load();

                        ApiKey = Env.GetString("API_KEY");
                        Bucket = Env.GetString("BUCKET");
                        AuthEmail = Env.GetString("AUTH_EMAIL");
                        AuthPassword = Env.GetString("AUTH_PASSWORD");
                        AuthDomain = Env.GetString("AUTH_DOMAIN");
                }

                public static string GenerisiUnikatanID()
                {
                        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                        var uuid = Guid.NewGuid();

                        var uniqueIdentifier = $"{timestamp}---{uuid}";

                        return uniqueIdentifier;
                }

                public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
                {

                        var config = new FirebaseAuthConfig
                        {
                                ApiKey = ApiKey,
                                AuthDomain = AuthDomain,
                                Providers = new FirebaseAuthProvider[] { new EmailProvider() }
                        };

                        try
                        {
                                var client = new FirebaseAuthClient(config);

                                var a = await client.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

                                var user = a.User;
                                var token = await user.GetIdTokenAsync();

                                var stream = fileStream;
                                var cancellation = new CancellationTokenSource();

                                var task = new FirebaseStorage(
                                                Bucket,
                                                new FirebaseStorageOptions
                                                {
                                                        AuthTokenAsyncFactory = () => Task.FromResult(token),
                                                        ThrowOnCancel = true
                                                })
                                        .Child(fileName + "---" + GenerisiUnikatanID())
                                        .PutAsync(stream, cancellation.Token);

                                task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

                                var downloadUrl = await task;

                                Console.WriteLine("Download link:\n" + downloadUrl);

                                return downloadUrl;
                        }
                        catch (Exception ex)
                        {
                                Console.WriteLine("Exception je bacen: {0}", ex);
                                return null;
                        }
                }

        }

}
