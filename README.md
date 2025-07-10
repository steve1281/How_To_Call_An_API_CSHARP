# How To Call an API in C sharp

## References

[How To Call An API in C#](https://www.youtube.com/watch?v=aWePkE2ReGw) by *IAmTimCorey*.
(note: I am not associated with, but do recommend)


## Create a simple WPF project
- call it ApiConsumerDemo
- I am using .NET 8

## Create MainWindow.xaml
- he doesn't cover this, so wing it.
- he provides code you can get by signing up for emails, if you want
- from what I can see, he has three buttons:

```
+------------------------------------------+
|   [Previous]  [Sun Information]   [Next] |
|------------------------------------------|
|         <image>                          |
...                                        |
+------------------------------------------+
```
- looking at the video a Grid layout should be used.
- lets try that, for now, without looking at his finished code.
- approx:
```
<Window x:Class="ApiConsumerDemo.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:ApiConsumerDemo"
        mc:Ignorable="d"
        Title="MainWindow" Height="450" Width="800">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="auto" />
            <RowDefinition Height="*" />
        </Grid.RowDefinitions>
        <StackPanel Grid.Row="0" Orientation="Horizontal" HorizontalAlignment="Center">
            <Button x:Name="previousImageButton" Padding="15" Margin="15">Previous</Button>
            <Button x:Name="sunInformationButton" Padding="15" Margin="15">Sun Information</Button>
            <Button x:Name="nextImageButton" Padding="15" Margin="15">Next</Button>
        </StackPanel>

        <Image Grid.Row="1" x:Name="comicImage" />
    </Grid>
</Window>
```
- will tune as needed

## Setup SunInfo.xaml

- looks like:
```
+-------------------------------------------+
| [            Load Sun Info               ]|
| +---------------------------------------+ |
| |   <sunrise Text>                      | |
| +---------------------------------------+ |
| +---------------------------------------+ |
| |   <sunet Text>                        | |
| +---------------------------------------+ |
+-------------------------------------------+
```
- as before, we can figure out what the xaml should be from video (approx)
- create a new window called SunInfo.   
- page or window? I actually don't know for sure, but I dont see tabbed panes or menus so popup seems likely
- something like:
```
<Window x:Class="ApiConsumerDemo.SunInfo"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:ApiConsumerDemo"
        mc:Ignorable="d"
        Title="SunInfo" Height="450" Width="800">
    <Grid>
        <StackPanel>
            <Button x:Name="loadSunInfo" Margin="20">Load Sun Info</Button>
            <TextBlock x:Name="sunriseText" Margin="20" />
            <TextBlock x:Name="sunsetText" Margin="20" />
        </StackPanel> 
    </Grid>
</Window>
```

## git intermission
- First time; generate a gitignore file, init, add ignore:

```
PS C:\Users\steve\wpf_projects\ApiConsumerDemo> dotnet new gitignore
The template "dotnet gitignore file" was created successfully.
```
- init
```
PS C:\Users\steve\wpf_projects\ApiConsumerDemo> git init
Initialized empty Git repository in C:/Users/steve/wpf_projects/ApiConsumerDemo/.git/
```
- add the ignore
```
PS C:\Users\steve\wpf_projects\ApiConsumerDemo> git add .gitignore
PS C:\Users\steve\wpf_projects\ApiConsumerDemo> git commit -m "add ignore file"
[master (root-commit) 566698d] add ignore file
 1 file changed, 484 insertions(+)
 create mode 100644 .gitignore
```
- ok, now lets save our UI stuff.
- status
```
PS C:\Users\steve\wpf_projects\ApiConsumerDemo> git status
On branch master
Untracked files:
  (use "git add <file>..." to include in what will be committed)
        ApiConsumerDemo.sln
        ApiConsumerDemo/

nothing added to commit but untracked files present (use "git add" to track)
```
- add/commit
```
PS C:\Users\steve\wpf_projects\ApiConsumerDemo> git add .
PS C:\Users\steve\wpf_projects\ApiConsumerDemo> git commit -m "setup simple UI"
[master 32cf5d1] setup simple UI
 9 files changed, 158 insertions(+)
 create mode 100644 ApiConsumerDemo.sln
 create mode 100644 ApiConsumerDemo/ApiConsumerDemo.csproj
 create mode 100644 ApiConsumerDemo/App.xaml
 create mode 100644 ApiConsumerDemo/App.xaml.cs
 create mode 100644 ApiConsumerDemo/AssemblyInfo.cs
 create mode 100644 ApiConsumerDemo/MainWindow.xaml
 create mode 100644 ApiConsumerDemo/MainWindow.xaml.cs
 create mode 100644 ApiConsumerDemo/SunInfo.xaml
 create mode 100644 ApiConsumerDemo/SunInfo.xaml.cs
```
- and yes, I know I can merge add and commit.  I choose not to because they are seperate actions.

## DemoLibrary
- So, this is a project added to the solution, *probably* to create a dll file where we can store some code
- He doesn't say its a WPF library, or just a C# library
- I have no idea what the difference is myself, so go ahead and add a C# Class library, call it DemoLibrary
- His is empty for now, so this should be ok as is.
- git intermission (since this is a discrete thing I may want to undo/edit)
```
PS C:\Users\steve\wpf_projects\ApiConsumerDemo> git status
On branch master
Untracked files:
  (use "git add <file>..." to include in what will be committed)
        DemoLibrary/

nothing added to commit but untracked files present (use "git add" to track)
PS C:\Users\steve\wpf_projects\ApiConsumerDemo> git add .
PS C:\Users\steve\wpf_projects\ApiConsumerDemo> git commit -m "add an empty class library called DemoLibrary"
[master c4b4955] add an empty class library called DemoLibrary
 2 files changed, 16 insertions(+)
 create mode 100644 DemoLibrary/Class1.cs
 create mode 100644 DemoLibrary/DemoLibrary.csproj
```
- yah, pretty sure that Class1.cs is not going to be in the end product


## Add ApiHelper
- right click the DemoLibrary, add New C# class 
- call it ApiHelper, and add
- switch internal to public. (why why do they have internal? what does that even mean?)

### sidebar, we need some packages
- references, manage nuget packages
- OK - I don't have a *references*
- Lets just add the package to the project. 
- The package is: *Microsoft.AspNet.WebApi.Client*
- Install that.
- He gets a request to update Newtonsoft.JSON.  I don't.
- installed:
```
Successfully installed 'Microsoft.AspNet.WebApi.Client 6.0.0' to DemoLibrary
Successfully installed 'Newtonsoft.Json 13.0.1' to DemoLibrary
Successfully installed 'Newtonsoft.Json.Bson 1.0.2' to DemoLibrary
Successfully installed 'System.Memory 4.5.5' to DemoLibrary
Successfully installed 'System.Threading.Tasks.Extensions 4.5.4' to DemoLibrary
```
### and back to the ApiHelper
- add prop (prop<tab>)
- property will be static (lets call it a singleton).  Should have one per application.
- some suspiciousness. 
- My code doesn't seem to need *System.Net.Http* . Which is odd; where is the IDE getting HttpClient from?
- also, it insists that I mark ApiClient property as nullable.
- is this a .net version change? Probably.
- resulting code:
```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DemoLibrary
{
    public static class ApiHelper
    {
        public static HttpClient? ApiClient { get; set; }
        public static void InitializeClient()
        {
            ApiClient = new HttpClient();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));


        }
    }
}
```
- pretty simple
- he also talkes about setting a Base URI; we arent doing that for this demo
- ok, git intermission:
```
PS C:\Users\steve\wpf_projects\ApiConsumerDemo> git status
On branch master
Changes not staged for commit:
  (use "git add/rm <file>..." to update what will be committed)
  (use "git restore <file>..." to discard changes in working directory)
        modified:   ApiConsumerDemo.sln
        deleted:    DemoLibrary/Class1.cs

Untracked files:
  (use "git add <file>..." to include in what will be committed)
        DemoLibrary/ApiHelper.cs

no changes added to commit (use "git add" and/or "git commit -a")
PS C:\Users\steve\wpf_projects\ApiConsumerDemo> git add .
PS C:\Users\steve\wpf_projects\ApiConsumerDemo> git commit -m "remove Class1.cs, add ApiHelper class"
[master 571753d] remove Class1.cs, add ApiHelper class
 3 files changed, 30 insertions(+), 7 deletions(-)
 create mode 100644 DemoLibrary/ApiHelper.cs
 delete mode 100644 DemoLibrary/Class1.cs
```

## Initialization in MainWindow.xaml.cs
- code:
```

namespace ApiConsumerDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ApiHelper.InitializeClient(); // <=== HERE

        }
    }
}
```
- you can test run , nothing will happen of course, but no errors
- git intermission ...

## add code for loading comic
- add a class to the DemoLibrary - ComicProcessor
### sidebar; need a ComicModel
- add a class to the DemoLibrary - ComicModel
- add two properties, int Num and string Img
```
namespace DemoLibrary
{
    public class ComicModel
    {
        public int Num { get; set; }
        public string Img { get; set; }

    }
}
```
### back to ComicProcessor
- the class we just made lines up with a subset of the returned JSON.
- so, NewtonConvertor will parse out the matching fields for us.
```
namespace DemoLibrary
{
    public class ComicProcessor
    {

        public async Task<ComicModel> LoadComic(int comicNumber=0)
        {
            // 0 will equate to the current comic
            string url = "";

            if (comicNumber > 0)
            {
                url = $"https://xkcd.com/{comicNumber}/info.0.json";
            } else
            {

                url = $"https://xkcd.com/info.0.json";
            }

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode) 
                {
                    ComicModel comic = await response.Content.ReadAsAsync<ComicModel>();

                    return comic;

                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

        }
    }
}
```
- git intermission

## form updates to call and load xkcd image
- MainWindow.xaml.cs add in LoadImage 
- add :
```
       private async Task LoadImage(int imageNumber =0)
       {
           var comic = await (new ComicProcessor()).LoadComic(imageNumber);
           if (imageNumber == 0)
           {
               maxNumber = comic.Num;
           }
           currentNumber = comic.Num;
           var uriSource = new Uri(comic.Img, UriKind.Absolute);
           comicImage.Source = new BitmapImage(uriSource);
       }
```
- next we modify the form script
- add in Loaded="Window_Loaded"
- this adds a method to your code behind called Windows_Loaded
- this can be made async, and can call your LoadImage (basically so you can test, or have a default)
- code:
```
       private async void Window_Loaded(object sender, RoutedEventArgs e)
       {
           await LoadImage();
       }
```
- git intermission

## wire up previous/next buttons
- code:
```

        private async void previousImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentNumber>1)
            {
                currentNumber--;
                nextImageButton.IsEnabled = true;
                await LoadImage(currentNumber);
                if (currentNumber ==1)
                {
                    previousImageButton.IsEnabled = false;
                }
            }
        }

        private async void nextImageButton_Click(object sender, RoutedEventArgs e)
        {
            if ( currentNumber<maxNumber)
            {
                currentNumber++;
                previousImageButton.IsEnabled = true;
                await LoadImage(currentNumber);
                if (currentNumber==maxNumber)
                {
                    nextImageButton.IsEnabled=false;
                }
            }
        }
```
- and git intermission



## caching
- the json from xkcd doesn't change for days past, so we can keep history
- leaving this as a todo

## sunrise and sunset
- need a model, create a class SunResultModel in DemoLibrary
- need another model, create the SunModel in DemoLibrary
- why two classes? has to do with what the api returns:
```
    {
      "results":
      {
        "sunrise":"7:27:02 AM",
        "sunset":"5:05:55 PM",
        "solar_noon":"12:16:28 PM",
        "day_length":"9:38:53",
        "civil_twilight_begin":"6:58:14 AM",
        "civil_twilight_end":"5:34:43 PM",
        "nautical_twilight_begin":"6:25:47 AM",
        "nautical_twilight_end":"6:07:10 PM",
        "astronomical_twilight_begin":"5:54:14 AM",
        "astronomical_twilight_end":"6:38:43 PM"
      },
       "status":"OK",
       "tzid": "UTC"
    }
```
- observe the results sub-class in the response
- so the SunResultModel has sunrise and sunset (SunRise and SunSet)
- code:
```
namespace DemoLibrary
{
    public class SunResultModel
    {
        public DateTime SunRise { get; set; }
        public DateTime SunSet { get; set; }
    }
}
```
- and SunModel has results (Results)
- code:
```
namespace DemoLibrary
{
    public class SunModel
    {
        public SunResultModel? Results { get; set; }
    }
}

```
- next, we need a Processor for this
- add class called SunProcessor
- copy code from ComicProcessor
- massage code.
- in the meanwhile..
- code:
```
namespace DemoLibrary
{
    public class SunProcessor
    {
        public async Task<SunResultModel> LoadSunInformation()
        {
            string url = "https://api.sunrise-sunset.org/json?lat=48.4463684&lng=-89.2712642&date=today";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    SunModel result = await response.Content.ReadAsAsync<SunModel>();

                    return result.Results;

                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

        }
    }
}

```


### sidebar; lets hook up the button in the SunInfo.xaml form
- double the button to add a handler
- make the handler code async
- tune up code:
```
namespace ApiConsumerDemo
{
    public partial class SunInfo : Window
    {
        public SunInfo()
        {
            InitializeComponent();
        }

        private async void loadSunInfo_Click(object sender, RoutedEventArgs e)
        {
            
            var sunInfo = await (new SunProcessor()).LoadSunInformation();
            sunriseText.Text = $"Sunrise is at {sunInfo.SunRise.ToLocalTime().ToShortTimeString()}";
            sunsetText.Text = $"Sunset is at {sunInfo.SunSet.ToLocalTime().ToShortTimeString()}";

        }
    }
}

```

### sidebar; wire up MainWindow.xaml so the Sun Information button opens our SunInfo.xaml window
- double click the button to add a handler
- add code:
```
        private void sunInformationButton_Click(object sender, RoutedEventArgs e)
        {
            (new SunInfo()).Show();

        }
    }
```

## Notes:
- we used WPF in this case, but ANY .NET would do
- the DemoLibrary us just C# and nuget package
- any C# application could use the API
- this seperation of concerns (SOC) is the right way to do this; author and I agree here.
- the APIs in the above examples are OPEN API; no authentification.
- we didnt cover POST; there are differences from the GET.


## git log --oneline --decorate --all --reverse

- 566698d add ignore file
- 32cf5d1 setup simple UI
- c4b4955 add an empty class library called DemoLibrary
- 571753d remove Class1.cs, add ApiHelper class
- 4280e72 add code to initialize our apiclient
- 9054de1 add call to xkcd and a model to handle returned data
- 043e756 add in LoadImage and a Window_Loaded and tune up the form a bit
- 25e91ed connect up the next and prev buttons. enable buttons as needed
- 8278306 Added SunInfo logic and UI

