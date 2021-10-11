import React from 'react'

function StageTableRow({stage}) {
  return (
    <div>
      <tr>
        <td>{stage.id}</td>
        <td>{stage.name}</td>
      </tr>
    </div>
  );
}

export default StageTableRow
