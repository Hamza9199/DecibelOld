// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let odabranaPjesmaID;

async function ovo(playlistaID)
{
    if (odabranaPjesmaID == null)
        return;

    if (playlistaID == null)
        return;

    console.log(`odabranaPjesmaID: ${odabranaPjesmaID}, playlistaID: ${playlistaID}`);

    try
    {
        const response = await
            fetch(`/api/PjesmaPlayListaControllerAPI/DodajPjesmuNaKrajListe/${odabranaPjesmaID}/${playlistaID}`,
                {
                    method: "POST"
                });

        if (!response.ok)
        {
            const errorData = await response.json();

            if (errorData.message && errorData.message.includes("Violation of PRIMARY KEY constraint"))
            {
                alert("This song is already in the playlist!");
            } else
            {
                alert("An error occurred: " + errorData.message);
            }

            throw new Error(errorData.message || "An unknown error occurred.");
        }

        const json = await response.json();
        console.log(json);
    }
    catch (error)
    {
        console.error(error.message);
    }
}


const dodajPlaylistu = (id, event) =>
{
    const popup = document.getElementById('playlistPopup');

    if (popup == null)
    {
        return;
    }

    popup.style.display = 'block';
    popup.style.left = event.clientX - 300 + 'px';
    popup.style.top = event.clientY + 'px';
    odabranaPjesmaID = id;

    console.log(id);
};

/*
let sePopupVidi = false;

window.addEventListener('click', function (event)
{
    const popup = document.getElementById('playlistPopup');

        console.log(event.target);
    if (event.target != popup)
    {
        if (sePopupVidi)
        {
            popup.style.display = `none`;

            sePopupVidi = false;
            return;
        }

        sePopupVidi = true;

    } else if (event.target.classList.contains('fa-solid fa-plus')) {
        console.log("POLUDI");
    }
});
*/

function setSelectedTrackId(trackId)
{
    document.getElementById('selectedTrackId').value = trackId;
}


function addToPlaylist(playlistId)
{
    var trackId = document.getElementById('selectedTrackId').value;

    $.ajax({
        url: '/PjesmaPlayLista/DodajPjesmuNaKrajListe',
        type: 'POST',
        data: {
            pjesmaID: trackId,
            playlistaID: playlistId
        },
        success: function (response)
        {
            if (response.success)
            {
                showToast(response.message, 'success'); // Show success toast

                $('#exampleModal').modal('hide');
            }
        },
        error: function (xhr)
        {
            var response = JSON.parse(xhr.responseText);
            alert(response.message);
        }
    });
}

async function promjeniRedoslijed(pjesmaID, pocetnaPjesmaID, playlistaID)
{
    try
    {
        const response = await fetch('/PromjeniRedoslijed', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                PjesmaID: pjesmaID,
                PocetnaPjesmaID: pocetnaPjesmaID,
                PlaylistaID: playlistaID,
            }),
        });

        const data = await response.json();

        if (data.success)
        {
            showToast(data.message, 'success'); // Show success toast
        } else
        {
            showToast(data.message, 'danger'); // Show error toast
        }
    } catch (error)
    {
        console.error('Error sending request:', error);
        showToast('An unexpected error occurred. Please try again.', 'danger'); // Show error toast
    }
}

async function premjestiNaPocetak(pjesmaID, playlistaID)
{
    try
    {
        const response = await fetch('/PremjestiNaPocetak', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                PjesmaID: pjesmaID,
                PlaylistaID: playlistaID,
            }),
        });

        const data = await response.json();

        if (data.success)
        {
            showToast(data.message, 'success'); // Show success toast
        } else
        {
            showToast(data.message, 'danger'); // Show error toast
        }
    } catch (error)
    {
        console.error('Error sending request:', error);
        showToast('An unexpected error occurred. Please try again.', 'danger'); // Show error toast
    }
}


// Example function to update the song list dynamically
function updateSongList(updatedOrder)
{
    const songList = document.getElementById("sortable-song-list");
    songList.innerHTML = ''; // Clear existing list

    updatedOrder.forEach(songId =>
    {
        const listItem = document.createElement("li");
        listItem.textContent = `Song ID: ${songId}`;
        songList.appendChild(listItem);
    });
}


function showToast(message, type = 'success')
{
    const toastContainer = document.getElementById('toastContainer');

    // Create a new toast element
    const toast = document.createElement('div');
    toast.className = `toast align-items-center toast-dark-gray border-0 position-fixed top-0 end-0 p-3`;
    toast.style.backgroundColor = '#333'; // Dark gray
    toast.style.color = 'white'; // Ensure text is readable
    toast.style.zIndex = '1055'; // Ensure it appears above other elements
    toast.role = 'alert';
    toast.ariaLive = 'assertive';
    toast.ariaAtomic = 'true';
    toast.innerHTML = `
    <div class="d-flex">
        <div class="toast-body">
            ${message}
        </div>
        <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
    </div>
`;


    // Append the toast to the container
    toastContainer.appendChild(toast);

    // Initialize and show the toast
    const bootstrapToast = new bootstrap.Toast(toast);
    bootstrapToast.show();

    // Remove the toast from the DOM after it’s hidden
    toast.addEventListener('hidden.bs.toast', () =>
    {
        toast.remove();
    });
}

