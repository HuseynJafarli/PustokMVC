let basketBtns = document.querySelectorAll(".add-to-basket")

basketBtns.forEach(btn => {
    btn.addEventListener("click", function (e) {
        e.preventDefault();

        let url = btn.getAttribute("href")

        fetch(url)
            .then(response => {
                if (response.status == 200) {
                    alert("Added!")
                } else {
                    alert("Book Not Found!")
                }
            })
    })
})