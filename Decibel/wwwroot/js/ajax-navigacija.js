document.addEventListener("DOMContentLoaded", function ()
{
    // Dobavlja .main-content div _Layout stranice
    const mainContent = document.getElementById("main-content");

    // Postavlja flag učitavanja na false
    let isLoading = false;

    // Postavlja event listener 'click' na dokument
    document.addEventListener('click', function (e)
    {
        // Pronalazi najbližeg roditelja elementa koji sadrži klasu .ajax-link
        // Generalno kada HTML element unutar svoje klase sadrži .ajax-link (class="ajax-link")
        // Ovo preventuje full reload stranice da se audio player ne bi zaustavio tokom
        // navigacije na ostale stranice website-a
        const link = e.target.closest(".ajax-link");
        const linkNoPush = e.target.closest(".ajax-link-no-push");

        console.log(`${link}, ${linkNoPush}`);

        // AKO postoji taj element
        if (link)
        {
            // Preventuje default behaviour što je full reload stranice
            e.preventDefault();

            // Dobavlja URL anchor elementa preko href atributa (anchor koji ima class selector = .ajax-link)
            // <a class="ajax-link"></a>
            const url = link.getAttribute("href");

            // Ako se već obavlja proces ajax navigacije preventuje višestruki reload
            if (isLoading) return;

            // Postavlja flag na true ako je bio false (označava da je ušao u proces ajax navigacije)
            isLoading = true;

            // fetch API dobavlja HTML stranice na koji href URL pokazuje
            fetch(url)
                .then(response => response.text())
                .then(html =>
                {
                    // Kreira privremeni div i puni ga HTML-om nove stranice
                    const tempDiv = document.createElement("div");
                    tempDiv.innerHTML = html;

                    // Dobavlja sadržaj <main> elementa nove stranice i zamjenjuje trenutni
                    if (tempDiv.querySelector("main") == null)
                        return;

                    const newContent = tempDiv.querySelector("main").innerHTML;

                    if (newContent == null)
                        return;

                    mainContent.innerHTML = newContent;

                    // Poziva funkciju za ponovno učitavanje skripti i stilova
                    reloadScriptsAndStyles(tempDiv);

                    // Sprema trenutnu poziciju skrola prije navigacije
                    if (linkNoPush == null) {
                        window.history.pushState({ scrollTop: window.scrollY }, "", url);
                    }
                })
                .catch(err => console.error("Greška pri učitavanju sadržaja:", err))
                .finally(() =>
                {
                    // Postavlja flag nazad na false nakon završetka ajax navigacije
                    isLoading = false;
                });
        }
    });

    // Dodaje event listener za popstate event (navigacija unazad/naprijed u historiji)
    window.addEventListener("popstate", function (event)
    {
        // Dobavlja trenutni URL iz historije
        const url = window.location.pathname;

        // Ako je već u procesu učitavanja, ne čini ništa
        if (isLoading) return;

        // Postavlja flag na true da označi proces učitavanja
        isLoading = true;

        // fetch API ponovo dobavlja HTML trenutne stranice
        fetch(url)
            .then(response => response.text())
            .then(html =>
            {
                // Kreira privremeni div za parsiranje nove stranice
                const tempDiv = document.createElement("div");
                tempDiv.innerHTML = html;

                // Ažurira sadržaj <main> elementa novim sadržajem
                const newContent = tempDiv.querySelector("main").innerHTML;
                mainContent.innerHTML = newContent;

                // Ponovo učitava sve potrebne skripte i stilove
                reloadScriptsAndStyles(tempDiv);

                // Vraća korisnika na prethodno spremljenu poziciju skrola
                if (event.state && event.state.scrollTop !== undefined)
                {
                    window.scrollTo(0, event.state.scrollTop);
                }
            })
            .catch(err => console.error("Greška pri učitavanju sadržaja:", err))
            .finally(() =>
            {
                // Postavlja flag nazad na false nakon završetka učitavanja
                isLoading = false;
            });
    });

    // Funkcija za ponovno učitavanje skripti i stilova
    function reloadScriptsAndStyles(tempDiv)
    {
        // Dobavlja sve <script> elemente nove stranice
        const newScripts = tempDiv.querySelectorAll("script");
        newScripts.forEach(script =>
        {
            // Ignorira skripte koje su već učitane (site.js, player.js)
            if (["site.js", "player.js", "jquery-3.2.1.slim.min.js", "popper.min.js", "bootstrap.min.js", "d3@7"].some(src => script.src.includes(src)))
            {
                return;
            }

            // Kreira novi <script> element i dodaje ga u <body>
            const newScript = document.createElement("script");
            if (script.src)
            {
                newScript.src = script.src;
            } else
            {
                newScript.textContent = script.textContent;
            }
            document.body.appendChild(newScript);
        });

        // Dobavlja sve <link> elemente za stilove nove stranice
        const newStylesheets = tempDiv.querySelectorAll('link[rel="stylesheet"]');
        newStylesheets.forEach(sheet =>
        {
            // Provjerava da li je stylesheet već učitan, ako nije, dodaje ga
            if (!document.querySelector(`link[href="${sheet.href}"]`))
            {
                const newLink = document.createElement("link");
                newLink.rel = "stylesheet";
                newLink.href = sheet.href;
                document.head.appendChild(newLink);
            }
        });
    }
});

