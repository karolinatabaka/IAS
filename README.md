# IAS
Integracja aplikacji i systemów

# Członkowie grupy
## 149051 - Karolina Tabaka
- Projekt modeli danych, 
- Implementacja pierwszego Providera, 
- Stworzenie repozytorium oraz dokumentacji

## 149038 - Paulina Przybyszewska 
- Implementacja drugiego Providera, 
- Implmentacja HUB,
- Implementacja Generatora Danych dla Providera 1 i 2

## XXXXXX - Tomasz Tomaszewski 
- Implementacja API,
- Implementacja Klienta
- Stylizowanie Klienta + Implemetnacja mechanizmu wyswietlania losowych obrazków dla filmów

# Użyte technologie / języki programowania
Projekt powstał w oparciu o technolgie Microsoft .NET.
- Środowisko: Visual Studio 2015 Community
- Api: ASP .NET MVC API (języki: C#, HTML, CSS, JSON)
- Client: ASP. NET MVC (języki: C#, HTML, CSS, JSON)
- Provider1: MSSQL  
- Provider2: MongoDB

# Schemat oraz opis architektury systemu
### Client 
Strona internetowa prezentująca dane użytkownikowi końcowemu. Komunikuje się wyłącznie z API.

### API 
Wykorzystuje HUB do pozyskiwania danych od providerów. Udostępnia endpoint'y konsumowane przez aplikacje klienckie zwracające dane w formacie JSON.

### HUB 
Integruje i udostępnia dane od różnych providerów. Działa w oparciu o uspójniony model danych aby wykluczyć różnice w modelach przychodzących od providerów.

### Provider 1 
Umożliwia odczyt danych z bazy MSSQL. Dodatkowo implmentuje generator danych wypełniających bazę.

### Provider 2 
Umożliwia odczyt danych z bazy MongoDB. Dodatkowo implmentuje generator danych wypełniających bazę.

![schemat.png](https://raw.githubusercontent.com/karolinatabaka/IAS/master/Docs/schemat.PNG)

# Opis dostawców, struktura encji

## Provider 1 
Baza MSSQL. Wymaga instalacji środowiska MSSQL Server Express + MSSQL Server Management Studio. Integracja z bazą następuje bezpośrednio z poziomu Visual Studio za pomocą wbudowanych providerów. Na bazie struktury danych tworzony jest automatyczny model, wykorzystywany następnie w implmentacji logiki pozostałej części aplikacji.

### Struktura encji
```
namespace ProviderOne
{
    using System;
    
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public System.DateTime PublishDate { get; set; }
        public string ShortDescription { get; set; }
    }
}
```
## Provider 2 
Baza MongoDB. Wymaga instalacji servera MongoDB. Integracja z bazą następuje za pomocą dostępnych bibliotek integrujących MongoDB ze środowiskiem .NET. Odwrotnie niż w przypadku MSSQL model został stworzony najpierw, a dopiero bazując na jego architekturze MongoDB utworzył odpowiadającą mu strukturę. 

### Struktura encji
```
namespace ProviderTwo
{
    public class Movie
    {
        [BsonId]
        public MongoDB.Bson.ObjectId Id { get; set; } 

        public int Identifier { get; set; }

        public string Title { get; set; }

        public string Description_part1 { get; set; }

        public string Description_part2 { get; set; }

        public int PublishYear { get; set; }

        public int PublishMonth { get; set; }

        public int PublishDay { get; set; }

    }
}
```

# Opis HUB'a
Głównym zadaniem HUB'a jest integracja i uspójnianie danych otrzymywanych od różnych providerów.
Wspierane akcje: GetMovies, GetByID

## Przebieg integracji encji
Na przykładzie akcji GetMovies
```
public IEnumerable<HubMovie> GetMovies()
        {
            var moviesFromProviderOne = providerOneApi.GetMovies().Select(x=> new HubMovie() {
                Id = x.Id,
                Description = x.ShortDescription.Trim() + " " + x.Genre,
                PublishDate = x.PublishDate,
                Name = x.Name,
                Provider = Provider.ProviderOne
            });

            var moviesFromProviderTwo = providerTwoApi.GetMovies().Select(x=> new HubMovie()
            {
                Id = x.Identifier,
                Description = x.Description_part1 + " " + x.Description_part2,
                PublishDate = new DateTime(x.PublishYear, x.PublishMonth, x.PublishDay),
                Name = x.Title,
                Provider = Provider.ProviderTwo
            });

            return moviesFromProviderOne.Union(moviesFromProviderTwo);

        }
```
## Struktura wynikowa encji
```
namespace HUB
{
    public class HubMovie
    {
        public int Id { get; set; }

        public string Name { get;set;}

        public string Description { get; set;}

        public DateTime PublishDate { get; set; }

        public Provider Provider { get; set; }
    }
}
```

# Napotkane problemy

## Integracja z MongoDB
MongoDB nie jest typową bazą danych. Główne różnice między MongoDB a relacyjnymi bazami danych to między innymi
- Brak SQL ( Mongo używa własnego języka zapytań )
- Brak schematu bazy danych
Ze względu na te różnice na poziomie integracji z bazą MongoDB napotkaliśmy problemy z jego sposobem na przechowywanie i generwanie kluczy głównych oraz to jak je odzwierciedliś na poziomie imeplementacji.

## Separacja logiki zwracającej dane w zależności od providera
Zazwyczaj akcje zwracające szczegóły danej encji implementowane są na zasadzie GetById z jednym parametrem wejściowym Id. Ze względu na specyfikację projektu i fakt korzystania z wielu providerów to podejście musiało zostać przełamane. Struktura wynikowa encji została poszerzona o możliwość zapisu swojego pochodzenia (providera). 

Dzięki temu na całej drodze komunikacji zaczynając po stronie klienta końcowego, zapytanie do API posiadało poszerzony kotekst:
```
public HubMovie Get(Provider provider, int id)
{
    return hubProviderApi.GetMovie(id, provider);
}
```

# Adres do repozytorium

https://github.com/karolinatabaka/IAS

# Co byśmy zmienili gdybyśmy robili ten projekt jeszcze raz?
Na etapie implemetacji samego HUB. Warto byłoby wprowadzić pośedniczącą bazę danych. Rozwiązało by to problem poszerzenia kontekstu na poziomie zapytań do API zwracających szczegółowe dane filmu: Get(Provider provider, int id). Pośrednicząca baza danych miała by wlasne klucze główne więc akcja zwracjąca szczegółowe dane filmu mogłaby zostać uproszczona: Get(int id). Dodatkowo pośrednicząca baza danych stworzyła by miejsce na zaawansowaną komunikację z providerami (np. synchronizacja danych tylko raz na jakiś czas, co zmniejszyłoby obciążenie serwera).
