window.imprimirElemento = function (id) {

    const contenido = document.getElementById(id);

    if (!contenido) {
        console.error(`No existe el elemento ${id}`);
        return;
    }

    const ventana = window.open('', '_blank');

    ventana.document.write(`
        <html>
        <head>
            <title>Detalle Reposo</title>
        </head>
        <body>
            ${contenido.innerHTML}
        </body>
        </html>
    `);

    ventana.document.close();

    setTimeout(() => {
        ventana.print();
    }, 500);
};