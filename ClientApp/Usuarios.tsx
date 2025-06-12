import { useEffect, useState } from "react";

type Usuario = {
  "Id": string,
  "Nombre": string,
  "Password": string,
  "Correo": string
}

const Usuarios = () => {
    //Datos
    const[registros,setRegistros] = useState<Usuario[]>([]);
    const[texto,setTexto] = useState("");
    
    const listarRegistros = async () => {
       let url = "/api/usuarios";

       if (texto){
         url += "?texto=" + texto;
       }

        const resp = await fetch(url);
        if (resp.ok){
            const datos = await resp.json();
            setRegistros(datos);
        }
    }

    useEffect(()=>{
        listarRegistros(); 
    } , []);

    const buscar = () =>{
        listarRegistros();
    }

    //Vista
    return (
        <>
        <div className="container">
            <h1>Usuarios</h1>
        </div>
        <div className="container">
            <div className="card">
                <div className="card-header">Búsqueda</div>
                <div className="card-body">
                    <div className="row">
                        <div className="col-12 ">
                            <div className="mb-3">
                                <label>Búsqueda de Usuarios</label>
                                <input type="text" className="form-control" 
                                 placeholder="Introduce el nombre o el correo"
                                 onChange={(e) => setTexto(e.target.value)}/>
                           </div>
                        </div>
                        <div className="col-12">
                            <button className="btn btn-primary" onClick={buscar}>Buscar</button>
                        </div>
                    </div>
                </div>
           </div>
        </div>
        <div className="container">
             <div className="card">
                <div className="card-header">Usuarios existentes </div>
                <div className="card-body">
                    <table className="table table-striped">
                        <thead>
                            <tr>
                                <th>No.</th>
                                <th>Nombre</th>
                                <th>Correo</th>
                                <th>Password</th>
                                <th></th>
                            </tr>
                        </thead>
                        {
                            registros.length === 0 &&
                            <tbody>
                                <tr>
                                    <td colSpan={5}>No hay registros para mostrar</td>
                                </tr>
                            </tbody>
                        }
                        {
                            registros.length > 0 &&
                            <tbody>
                                {
                                    registros.map((item, index ) => 
                                    <tr>
                                    <td>{index + 1}</td>
                                    <td>{item.Nombre}</td>
                                    <td>{item.Correo}</td>
                                    <td>{item.Password}</td>
                                    <td className="d-flex gap-2">
                                        <a className="btn btn-primary" href={"/usuarios/" + item.Id}>Editar</a>
                                        <button className="btn btn-danger">Eliminar</button>
                                    </td>
                                   </tr>
                                    )
                                }
                            </tbody>
                        }
                    </table>
                </div> 
            </div>
        </div>
        
        </>
    )
}











export default Usuarios;