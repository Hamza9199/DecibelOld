let player = document.getElementById('audio-player');
let progress = document.getElementById("progress");
let kontrole = document.getElementById("play-button");
let glasnoca = document.getElementById("glasnoca");
let glasnocaIkona = document.getElementById("glasnoca-ikona");
let trenutnoPjesma = document.getElementById("trenutno-pjesma");
let trajanjePjesme = document.getElementById("trajanje-pjesme");
let glasnocaProcenat = document.getElementById("glasnoca-procenat");
let slikaPjesmeLijevo = document.getElementById("slika-pjesme-lijevo");
let nazivPjesme = document.getElementById("naziv-pjesme");
let izvodjaciPjesme = document.getElementById("izvodjaci-pjesme");
let odaklePusta = document.getElementById("odakle-pusta");
const petljaTipka = document.getElementById("loop-button");
let pozadinaPjesmaList = document.getElementById("pozadina-pjesma-list");
let slikaPjesmeLijevoA = document.getElementById("slika-pjesme-lijevo-a");
let lajkovanoGlobal = document.getElementById("lajkovano-global");
let nextTipka = document.getElementById("next-tipka");
let prevTipka = document.getElementById("prev-tipka");
let isLooping = false;

let prev = -1;
let next = -1;
let context = "Ostalo";
let contextURL = "/Pjesma";
let playlista = -1;

player.volume = 0.2;

const playPause = () =>
{
    if (kontrole.classList.contains("fa-pause"))
    {
        _pause();
    } else
    {
        _play();
    }
}

const _play = async (data) =>
{
    player.play();

    kontrole.classList.remove("fa-play");

    kontrole.classList.add("fa-pause");


    console.log(data);

    console.log(JSON.stringify(data));

    try
    {
        const response = await fetch('/HistorijaSlusanja/PushSesija', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        if (response.ok)
        {
            console.log("Historija pustanja (Sesija) sacuvana uspjesno.");
        } else
        {
            console.error("ERROR", response.message);
            console.error(response);
        }
    } catch (error)
    {
        console.error("ERROR: ", error);
    }
}

const _pause = () =>
{
    player.pause();

    kontrole.classList.remove("fa-pause");

    kontrole.classList.add("fa-play");
}

const _povecajGlasnocuZa = (arg) => 
{
    if (arg + player.volume > 1.0)
    {
        player.volume = 1.0;
        return;
    }

    player.volume += arg;

    _updateGlasnoca();
}

let isUpArrowPressed = false;
let isDownArrowPressed = false;

document.addEventListener('keydown', (e) =>
{
    if (e.code === 'Space')
    {
        console.log(e.code);

        playPause();
    }

    if (e.code === 'ArrowUp')
    {
        isUpArrowPressed = true;
    }

    if (isUpArrowPressed && e.shiftKey)
    {
        _povecajGlasnocuZa(0.05);
        _updateGlasnocaProcenat();
        isUpArrowPressed = false;
        return;
    }

    if (e.code === 'ArrowDown')
    {
        isDownArrowPressed = true;
    }

    if (isDownArrowPressed && e.shiftKey)
    {
        _smanjiGlasnocuZa(0.05);
        _updateGlasnocaProcenat();
        isDownArrowPressed = false;
        return;
    }

    if (e.code === 'ArrowRight')
    {
        _seekZaKoliko(5);
        return;
    }

    if (e.code === 'ArrowLeft')
    {
        _seekZaKoliko(-5);
        return;
    }

});

const _smanjiGlasnocuZa = (arg) =>
{
    if (player.volume - arg < 0.0)
    {
        player.volume = 0.0;
        return;
    }

    player.volume -= arg;

    _updateGlasnoca();
}

const _updateGlasnoca = () =>
{
    glasnoca.value = parseInt(player.volume * 100) - 1;

    console.log(parseInt(player.volume * 100));
}

const _promjeniGlasnocu = (arg) =>
{
    player.volume = arg;
}

const _seekZaKoliko = (arg) =>
{
    player.currentTime += arg;
}

const promjeniPjesmu = (putanjaAudio, putanjaSlika, putanjaGIF, naziv, izvodjaci, pjesmaID, lajkovana, context_, contextURL_, prevID, nextID, playlistaID) =>
{
    promjeniPlayerDetalje(putanjaSlika, putanjaGIF, naziv, izvodjaci, pjesmaID, lajkovana, context_, contextURL_, prevID, nextID, playlistaID);

    prev = prevID;
    next = nextID;
    context = context_;
    contextURL = contextURL_;
    playlista = playlistaID;

    console.log("PREV: ", prev);
    console.log("NEXT: ", next);

    let bg;

    if (putanjaGIF != null)
        document.getElementById("pozadina-pjesma-list").style.backgroundImage = `url("${putanjaGIF}")`;
    else
        document.getElementById("pozadina-pjesma-list").style.backgroundImage = `url("${putanjaSlika}")`;

    player.src = putanjaAudio;

    player.load();

    progress.max = parseInt(player.duration);

    progress.value = parseInt(player.currentTime);

    const data = {
        pjesmaID: pjesmaID,
        playlistaID: null,
        trajanje: 0,
        kontekstPustanjaURL: contextURL,
        kontekstPustanja: context,
        offline: false
    };

    _play(data);

}

const promjeniPlayerDetalje = (putanjaSlika, putanjaGIF, naziv, izvodjaci, pjesmaID, lajkovana, context, contextURL, prev, next) =>
{
    console.log("Promijenjeno!");

    if (context != null)
    {
        odaklePusta.innerText = `Iz: ${context}`;
        odaklePusta.setAttribute("href", `${context}`);
    }

    if (lajkovana == null)
        console.log("NULL");

    if (lajkovana === true)
    {
        console.log("OVDJE!");
        lajkovanoGlobal.classList.remove("fa-regular");
        lajkovanoGlobal.classList.add("fa-solid");
    } else
    {
        lajkovanoGlobal.classList.remove("fa-solid");
        lajkovanoGlobal.classList.add("fa-regular");
    }

    if (putanjaGIF != null)
        slikaPjesmeLijevo.src = putanjaGIF;
    else
        slikaPjesmeLijevo.src = putanjaSlika;

    nazivPjesme.innerHTML = `<a class="ajax-link" href="/Pjesma/Details/${pjesmaID}">${naziv}</a>`;

    slikaPjesmeLijevoA.getAttribute("href");

    slikaPjesmeLijevoA.setAttribute("href", `/Pjesma/Details/${pjesmaID}`);

    izvodjaciPjesme.innerHTML = ``;

    if (Array.isArray(izvodjaci) && izvodjaci.length > 0)
    {
        izvodjaci.forEach((i, index) =>
        {
            console.log(i.korisnickoIme);

            izvodjaciPjesme.innerHTML += `<a class="ajax-link" href="/Korisnik/Details/${i.id}">${i.korisnickoIme}</a>`;

            if (index < izvodjaci.length - 1)
            {
                izvodjaciPjesme.innerHTML += `, `;
            }
        });
    } else
    {
        console.error("izvodjaci is not an array or is empty");
        izvodjaciPjesme.innerHTML = `<span>No performers available</span>`;
    }

    if (typeof attachLinkHandlers === "function")
    {
        attachLinkHandlers();
    }
}

player.addEventListener('timeupdate', () =>
{
    progress.value = 100 * (player.currentTime / player.duration);

    _updateVrijeme(parseInt(player.currentTime));


});

progress.addEventListener('input',
    () =>
    {
        player.play();

        player.currentTime = (progress.value) * (player.duration / 100);
    });

glasnoca.addEventListener('input',
    () =>
    {
        player.volume = glasnoca.value / 100;

        _updateGlasnocaProcenat();
    });

let trenutnaGlasnoca = player.volume;

const mute = () =>
{
    if (player.volume === 0.0)
    {
        _mute();
    } else
    {
        _unMute();
    }

    _updateGlasnoca();
    _updateGlasnocaProcenat();

}

const _mute = () =>
{
    glasnocaIkona.classList.remove("fa-volume-xmark");

    glasnocaIkona.classList.add("fa-volume-high");

    player.volume = trenutnaGlasnoca;

    _promjeniGlasnocu(trenutnaGlasnoca);
}

const _unMute = () =>
{
    glasnocaIkona.classList.remove("fa-volume-high");

    glasnocaIkona.classList.add("fa-volume-xmark");

    trenutnaGlasnoca = player.volume;

    _promjeniGlasnocu(0);
}

const _updateVrijeme = (arg) =>
{
    trenutnoPjesma.innerText = formatirajVrijeme(arg);

    trajanjePjesme.innerText = formatirajVrijeme(parseInt(player.duration));
}

const formatirajVrijeme = (sek) =>
{
    const sati = Math.floor(sek / 3600);
    const minute = Math.floor((sek % 3600) / 60);
    const sekunde = sek % 60;

    if (sati > 0)
    {
        return `${sati}:${minute.toString().padStart(2, '0')}:${sekunde.toString().padStart(2, '0')}`;
    } else
    {
        return `${minute}:${sekunde.toString().padStart(2, '0')}`;
    }
}

const _updateGlasnocaProcenat = () =>
{
    glasnocaProcenat.innerText = `${parseInt(player.volume * 100)}%`;
}

const petlja = () =>
{
    console.log("CLICKED!");
    isLooping = !isLooping;
    player.loop = isLooping;

    if (isLooping)
    {
        petljaTipka.classList.add("active");
    } else
    {
        petljaTipka.classList.remove("active");
    }
}


petljaTipka.addEventListener('click', (e) =>
{
    petlja();
});

kontrole.addEventListener('click', (e) =>
{
    playPause();
});

glasnocaIkona.addEventListener('click', (e) =>
{
    mute();
});

nextTipka.addEventListener('click', (e) =>
{
    console.log("Next");

    prom(playlista, next);
});

prevTipka.addEventListener('click', (e) =>
{
    console.log("Prev");

    prom(playlista, prev);
});

const prom = async (playlistaID, pjesmaID) => {
    console.log("PJESMA_ID: ", pjesmaID);
    console.log("PLAYLISTAID: ", playlistaID);

    const response = await fetch(`/PjesmaPlayLista/GetPjesmaPlayListaViewModel/${playlistaID}/${pjesmaID}`);

    if (!response.ok)
    {
        throw new Error(`Nije pronađena pjesma: ${response.status} ${response.statusText}`);
    }

    const data = await response.json();

    console.log(data);

    promjeniPjesmu(data.putanjaAudio, data.putanjaSlika, data.putanjaGIF, data.naziv, data.izvodjaci, pjesmaID, data.lajkovana, context, contextURL, data.prev, data.next, playlista);
}
