const formQuarto = document.getElementById("quartoForm");
const apiURLHotel = "http://localhost:5195/api/Hoteis"
const apiURLQuarto = "http://localhost:5195/api/Quartos"
const divCards = document.getElementById("cards")

async function buscarQuartos() {
    try{
        const resposta = await fetch(apiURLQuarto);
        if(!resposta.ok){
            throw new Error(`Erro HTTP: ${resposta.status}`);
        }
        const dados = await resposta.json();
        // divCards.innerHTML = "";
        dados.forEach((dado) => {
            //Criar elementos HTML para exibir os dados
            let card = document.createElement("div");
            card.innerHTML = 
            `<h2>${dado.hotelId}</h2>
            <p>Tipo: ${dado.tipo}</p>
            <p>Preço: ${dado.preco}</p>`;
            divCards.appendChild(card);
        });
    } catch (error){
        console.log("Erro ao buscar os dados:", error);
    }
}

async function cadastrarQuarto(event){
    event.preventDefault();
    const hotelId = document.getElementById("hotel").value;
    const tipo = document.getElementById("tipo").value;
    const preco = document.getElementById("preco").value;
    try{
        const resposta = await fetch(apiURLQuarto, {
            method: "POST", 
            headers:{
                "Content-Type" : "application/json" 
            },
            body: JSON.stringify({
                hotelId: hotelId,
                tipo: tipo,
                preco: preco,
            })
        });
        console.log("Resposta da API:", resposta)
        if(!resposta.ok){
            throw new Error ("Erro ao cadastrar quarto");
        }
        const dados = await resposta.json
        console.log("Quarto cadastrado com sucesso:", dados);
        formQuarto.reset();
        await buscarQuartos();
    } catch(error){
        console.log("Erro ao cadastrar hoteis:", error);
    }
}

async function listarHoteis(event){
    try{
        const resposta = await fetch(apiURLHotel);
        if(!resposta.ok){
            throw new Error(`Erro HTTP: ${resposta.status}`);
        }

    const dados = await resposta.json();
        
        const selectHotel = document.getElementById("hotel");
        selectHotel.innerHTML = "<option value = ''>Selecione um hotel </option>";
        dados.forEach((dado) => {
            const option = document.createElement("option");
            option.value =dado.id;
            option.textContent = dado.nome;
            selectHotel.appendChild(option);
        });
        } catch (error){
        console.log("Erro ao buscar os dados:", error);
    }
}
buscarQuartos();
formQuarto.addEventListener("submit", cadastrarQuarto);
listarHoteis();
