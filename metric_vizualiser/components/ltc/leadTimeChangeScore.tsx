import * as d3 from "d3";
import { useEffect, useRef } from "react";
import { ILeadTimeChange } from "types";
import { getHoursfromSeconds } from "utils";
type LeadTimeChangeProps = { data?: ILeadTimeChange; months: number };
export function LeadTimeChangeScore({ data, months }: LeadTimeChangeProps) {
  const headerRef = useRef(null);

  useEffect(() => {
    if (data === undefined) {
      return;
    }

    const [category, color, textColor] = ltc_category(
      data.medianLeadTimeChange
    );

    const width = 200,
      height = 200,
      radius = 100,
      side = 2 * radius * Math.cos(Math.PI / 4),
      dx = radius - side / 2;
    const svg = d3
      .select("body")
      .append("svg")
      .attr("width", width)
      .attr("height", height);
    svg
      .append("circle")
      .attr("cx", width / 2)
      .attr("cy", height / 2)
      .attr("r", radius)
      .attr("fill", color)
    const g = svg.append("g").attr("transform", "translate(" + [dx, dx] + ")");
    g.append("text")
      .attr("x", 75)
      .attr("y", 80)
      .attr("font-size", "40px")
      .attr("text-anchor", "middle")
      .attr("font-family", "sans-serif")
      .text(category)
      .attr("fill", textColor);
    const heightt = parseInt(
      g.select("text").node().getBoundingClientRect().heightt
    );

    g.select("text").attr("transform", "translate(0, " + -heightt / 2 + ")");

    return () => {
      svg.remove(), g.remove();
    };
  }, [data, months]);

  return <div ref={headerRef}></div>;
}

export function ltc_category(median: number) {
  median = getHoursfromSeconds(median);
  if (median <= 24) {
    return ["One Day", "rgba(106, 29, 114, 0.75)", "white"];
  } else if (median <= 24 * 7) {
    return ["One Week", "rgba(11, 156, 49, 0.75", "white"];
  } else if (median <= 24 * 7 * 4) {
    return ["One Month", "rgba(255, 158, 0, 0.75)", "black"];
  } else if (median <= 24 * 7 * 4 * 6) {
    return ["Six Months", "rgba(255, 158, 0, 0.75)", "black"];
  } else {
    return ["One Year", "rgba(230, 0, 0, 0.75)", "white"];
  }
}
