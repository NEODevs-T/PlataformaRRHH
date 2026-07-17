window.graficoScroll = {

    moverIzquierda: function (id) {

        const el = document.getElementById(id);

        if (!el) return;

        el.scrollBy({
            left: -300,
            behavior: 'smooth'
        });
    },

    moverDerecha: function (id) {

        const el = document.getElementById(id);

        if (!el) return;

        el.scrollBy({
            left: 300,
            behavior: 'smooth'
        });
    }
};