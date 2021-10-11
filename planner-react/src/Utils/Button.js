
function Button(props){
    const myClass = `button ${props.type}`
    return(
    <button className={myClass} onClick={props.handleClick}>{props.children}</button>
    )
    }