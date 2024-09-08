let basketBtns = document.querySelectorAll(".add-to-basket")

basketBtns.forEach(btn => {
    btn.addEventListener("click", function (e) {
        e.preventDefault();

        let url = btn.getAttribute("href")

        fetch(url)
            .then(response => {
                console.log(response);
                if (response.status == 200) {
                    alert("Added!");
                    /*$('.cart-block').load('/Basket/InvokeComponent');*/
                    UpdateBasket();
                } else {
                    alert("Book Not Found!")
                }
            })
    })
})

function UpdateBasket() {
    fetch('/Home/BasketUpdate')
        .then(response => response.text())
        .then(html => {
            document.querySelector('.cart-block').innerHTML = html;
        })
        .catch(error => console.error('Error updating basket:', error));
}
