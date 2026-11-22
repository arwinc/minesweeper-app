// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

import { dotnet } from './_framework/dotnet.js'

const outElement = document.getElementById("out");
let currentColor = "";
let isBold = false;

const ansiColors = {
  '30': '#000000', '31': '#f44747', '32': '#6a9955', '33': '#d7ba7d',
  '34': '#569cd6', '35': '#c586c0', '36': '#4ec9b0', '37': '#d4d4d4',
  '90': '#808080', '91': '#ff6b6b', '92': '#89d185', '93': '#ffe066',
  '94': '#75beff', '95': '#e599f7', '96': '#66d9ef', '97': '#ffffff'
};

const appendAnsiText = (text) => {
  const parts = text.split(/(\x1b\[[0-9;]*m)/g);

  parts.forEach(part => {
    if (!part) return;
    const match = part.match(/^\x1b\[([0-9;]*)m$/);
    if (match) {
      const codes = match.at(1).split(";");
      codes.forEach(code => {
        if (code === "" || code === "0" || code === "39") {
          currentColor = "";
          isBold = false;
        } else if (code === "1") {
          isBold = true;
        } else if (code === "22") {
          isBold = false;
        } else if (ansiColors[code]) {
          currentColor = ansiColors[code]
        }
      });
    } else {
      const span = document.createElement("span");
      span.textContent = part;
      if (currentColor) span.style.color = currentColor;
      if (isBold) span.style.fontWeight = "bold";
      outElement.appendChild(span);
    }
  });
}

const { setModuleImports, getAssemblyExports, getConfig, runMain } = await dotnet
  .withDiagnosticTracing(false)
  .create();

outElement.textContent = "";

setModuleImports('main.js', {
  dom: {
    appendOutput: (text) => appendAnsiText(text),
  }
});

const config = getConfig();
const exports = await getAssemblyExports(config.mainAssemblyName);

// run the C# Main() method and keep the runtime process running and executing further API calls
await runMain();