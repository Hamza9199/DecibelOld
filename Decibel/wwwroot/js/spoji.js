const spoji = () => {
    document.addEventListener('click', function (event) {
        const popup = document.getElementById('playlistPopup');
        if (!popup.contains(event.target) && !event.target.classList.contains('add-to-playlist-btn')) {
            popup.style.display = 'none';
        }
    });

    document.addEventListener("DOMContentLoaded", () => {
        document.querySelectorAll(".song-list tbody tr").forEach((row, index) => {
            row.addEventListener("click", () => {
                playButtons.getAttribute("data-pjesma-id");
                console.log("HERE");
                selectSong(row);
            });
        });
    });

    document.addEventListener("DOMContentLoaded", () => {
        const songItems = document.querySelectorAll(".song-item");
        let selectedSong = null;
        

        songItems.forEach(item => {
            item.addEventListener("click", async () => {
                if (selectedSong) {
                    selectedSong.classList.remove("selected");
                }
                item.classList.add("selected");
                selectedSong = item;

                const pjesmaID = item.getAttribute("data-id");
                const response = await fetch(`/Pjesma/GetPjesmaViewModel/${pjesmaID}`);
                if (!response.ok) {
                    throw new Error(`Nije pronađena pjesma: ${response.status} ${response.statusText}`);
                }

                const data = await response.json();
                console.log(data);
                promjeniPjesmu(data.putanjaAudio, data.putanjaSlika, data.putanjaGIF, data.naziv, data.izvodjaci, pjesmaID, data.lajkovana);
            });
        });

        
    });


    document.getElementById("komentar-form").addEventListener("submit", async function (event) {
        event.preventDefault();

     

        
    });



    document.addEventListener("DOMContentLoaded", ucitajKomentare);


};

document.addEventListener("DOMContentLoaded", spoji);
window.spoji = spoji();