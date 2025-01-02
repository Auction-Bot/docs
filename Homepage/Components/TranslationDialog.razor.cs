using Homepage.Shared.Utilities;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using System.Text.Json;
using System.Xml.Linq;

namespace Homepage.Components;

public partial class TranslationDialog
{
    private string FileIcon => _fileLoaded 
        ? _fileInError ? Icons.Material.Filled.CancelPresentation : Icons.Material.Filled.FilePresent 
        : Icons.Material.Filled.UploadFile;
    private bool _isLoading;
    private bool _fileLoaded;
    private bool _fileInError;
    private string _locale = string.Empty;
    private string? _english = string.Empty;
    private string? _fileName = string.Empty;
    private string? _discordId = string.Empty;
    private string? _discordUser = string.Empty;
    private CancellationTokenSource? _cts;
    private List<TranslationItem> _translationItems = [];
    private readonly Dictionary<string, (string icon, string language)> _locales = new()
    {
        { "es-ES", (TranslationIcons.Spanish, "Spanish") },
        { "fr-FR", (TranslationIcons.French, "French") },
        { "de-DE", (TranslationIcons.German, "German") },
        { "it-IT", (TranslationIcons.Italian, "Italian") },
        { "pt-BR", (TranslationIcons.Portuguese, "Portuguese") },
        { "ru-RU", (TranslationIcons.Russian, "Russian") },
        { "ja-JP", (TranslationIcons.Japanese, "Japanese") },
        { "ko-KR", (TranslationIcons.Korean, "Korean") },
        { "zh-CN", (TranslationIcons.SimplifiedChinese, "Chinese (Simplified)") },
        { "zh-TW", (TranslationIcons.TraditionalChinese, "Chinese (Traditional)") },
        { "pl-PL", (TranslationIcons.Polish, "Polish") },
        { "tr-TR", (TranslationIcons.Turkish, "Turkish") },
        { "th-TH", (TranslationIcons.Thailand, "Thai") },
        { "hi-IN", (TranslationIcons.India, "Hindi") },
        { "uk-UA", (TranslationIcons.Ukraine, "Ukrainian ") },
        { "bg-BG", (TranslationIcons.Bulgaria, "Bulgarian") },
        { "el-GR", (TranslationIcons.Greece, "Greek") },
        { "cs-CZ", (TranslationIcons.Czech, "Czech") },
        { "nl-NL", (TranslationIcons.Dutch, "Dutch") },
        { "sv-SE", (TranslationIcons.Swedish, "Swedish") }
    };

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _english = await FileService.GetFileContentAsync($"{TranslationType}Strings.resx");
    }

    private async Task LoadTranslations()
    {
        _fileName = $"{TranslationType}Strings.{_locale}.resx";

        try
        {
            _isLoading = true;

            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            var content = await FileService.GetFileContentAsync(_fileName);

            if (content is null)
            {
                _isLoading = false;
                return;
            }

            using TextReader textReader = new StringReader(content!);
            var doc = XDocument.Load(textReader);
            var dataElements = doc.Descendants("data");

            _translationItems = dataElements.Select(element => new TranslationItem(element.Attribute("name")!.Value, element.Element("value")!.Value, GetExistingTranslation(element)))
                                            .ToList();
        }
        catch (Exception)
        {
            //do nothing
        }
        finally
        {
            _isLoading = false;
        }
    }

    private static string GetExistingTranslation(XElement element)
    {
        var name = element.Attribute("name")!.Value;
        var value = element.Element("value")!.Value;

        return name.Equals(value, StringComparison.OrdinalIgnoreCase) ? string.Empty : value;
    }

    private async Task DownloadFile()
    {
        var translationItems = new List<TranslationItem>();

        if (_translationItems.Count == 0)
        {
            using TextReader textReader = new StringReader(_english!);
            var doc = XDocument.Load(textReader);
            var dataElements = doc.Descendants("data");

            translationItems = dataElements.Select(element => new TranslationItem(element.Attribute("name")!.Value, "", element.Element("value")!.Value)).ToList();
        }
        else
        {
            translationItems = [.. _translationItems];
        }

        translationItems.Insert(0, new TranslationItem("Original English Text", "", "Tranlated Text"));

        var jsonDict = new Dictionary<string, string>();
        
        foreach (var item in translationItems)
            jsonDict[item.Key] = item.TranslatedValue;

        var content = JsonSerializer.Serialize(jsonDict, new JsonSerializerOptions { WriteIndented = true });

        await JsRuntime.InvokeVoidAsync("downloadFile", _fileName!.Replace("resx", "json"), "application/json", content);
    }

    private async Task UploadFile(InputFileChangeEventArgs e)
    {
        _fileInError = false;
        _isLoading = true;
        try
        {
            var file = e.File;
            var fileParts = e.File.Name.Split('.');

            if (fileParts.Length != 3 || fileParts[1] != _locale)
            {
                _fileInError = true;
                _isLoading = false;
                return;
            }

            var buffer = new byte[file.Size];

            await file.OpenReadStream().ReadAsync(buffer);
            
            var jsonString = System.Text.Encoding.UTF8.GetString(buffer);
            var translations = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);

            if (translations is null)
            {
                _fileInError = true;
                _isLoading = false;
                return;
            }

            using TextReader textReader = new StringReader(_english!);
            var doc = XDocument.Load(textReader);
            var dataElements = doc.Descendants("data");
            
            _translationItems = dataElements.Select(element => new TranslationItem(element.Attribute("name")!.Value, element.Element("value")!.Value, element.Attribute("name")!.Value)).ToList();

            foreach (var item in _translationItems)
            {
                if (translations.TryGetValue(item.Key, out string? translation) && 
                    !string.IsNullOrWhiteSpace(translation) && 
                    !translation.Equals(item.Key, StringComparison.OrdinalIgnoreCase))
                {
                    item.TranslatedValue = translation;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error uploading JSON file: {ex.Message}");
        }

        _isLoading = false;
    }

    private async Task LoginWithDiscord()
    {
        var clientId = Configuration["Discord:ClientId"];
        var redirectUri = Uri.EscapeDataString($"{Navigation.BaseUri}authentication/login-callback");
        var scope = "identify email";

        var discordUrl = $"{Configuration["Discord:Authority"]}" +
            $"?client_id={clientId}" +
            $"&redirect_uri={redirectUri}" +
            $"&response_type=token" +
            $"&scope={scope}";

        var currentUrl = Navigation.Uri.Replace(Navigation.BaseUri, "/").Split('?')[0];
        if (currentUrl != "/login")
        {
            await JsRuntime.InvokeVoidAsync("localStorage.setItem", "returnUrl", $"/docs{currentUrl}");
        }

        Navigation.NavigateTo(discordUrl, true);
    }

    private async Task Submit()
    {
        try
        {
            XDocument newDoc = new(
                new XElement("root",
                    new XElement("resheader",
                        new XAttribute("name", "resmimetype"),
                        new XElement("value", "text/microsoft-resx"))
                )
            );

            foreach (var item in _translationItems)
            {
                newDoc.Root!.Add(
                    new XElement("data",
                        new XAttribute("name", item.Key),
                        new XAttribute(XNamespace.Xml + "space", "preserve"),
                        new XElement("value", item.TranslatedValue)
                    )
                );
            }

            _locale = _locale.StartsWith("zh") ? _locale : _locale.Split('-')[0];

            var fileName = $"{TranslationType}Strings.{_locale}.resx";

#if DEBUG
            await JsRuntime.InvokeVoidAsync("downloadFile", fileName, "text/xml", newDoc.ToString());
#else
            var mainBranch = "master";
            var newBranch = $"{_locale}-Translation-{DateTime.UtcNow.Ticks}";

            await GitHubService.CreateBranchAsync(newBranch, mainBranch);
            await GitHubService.UploadFileAsync(newBranch, fileName, newDoc.ToString());
            await GitHubService.CreatePullRequestAsync(newBranch, $"Add {_locale} Translation", $"{_locale} translations submitted by [{_discordUser}](https://discord.com/users/{_discordId})", mainBranch);
#endif

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating RESX file: {ex.Message}");
         
            MudDialog!.Close(DialogResult.Ok(false));
        }

        MudDialog!.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog!.Cancel();

    public ValueTask DisposeAsync()
    {
        _cts?.Cancel();
        _cts?.Dispose();

        return default;
    }

    private class TranslationItem(string Key, string OriginalValue, string TranslatedValue)
    {
        public string Key { get; set; } = Key;
        public string OriginalValue { get; set; } = OriginalValue;
        public string TranslatedValue { get; set; } = TranslatedValue;
    }
}
