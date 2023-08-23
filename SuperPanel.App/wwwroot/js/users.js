
function OnPageItemClick(url, pageNumber) {
    let sizeSelect = document.getElementById("usersLengthOptions");
    let pageSize = sizeSelect.options[sizeSelect.selectedIndex].value;
    window.location.href = `${url}?page=${pageNumber}&pageSize=${pageSize}`;
}

function SetPageSize(pageSize) {
    const sizeSelect = document.getElementById('usersLengthOptions');
    sizeSelect.value = pageSize;
}