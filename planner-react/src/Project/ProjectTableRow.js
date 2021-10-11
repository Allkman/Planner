import React from 'react'

function ProjectTableRow({project}) {
    return (
        <tr>
            <td>{project.id}</td>
            <td>{project.name}</td>
        </tr>
    )
}

export default ProjectTableRow
