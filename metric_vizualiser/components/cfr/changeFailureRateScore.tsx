import * as d3 from "d3";
import { useEffect, useRef } from "react";
import { IChangeFailureRate } from "types";
type ChangeFailureRateProps = { data?: IChangeFailureRate; months: number };
export function ChangeFailureRateScore({
  data,
  months,
}: ChangeFailureRateProps) {
  const headerRef = useRef(null);

  useEffect(() => {
    if (data === undefined) {
      return;
    }

    const [category, color, textColor] = cfr_category(data.changeFailureRate);

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

export function cfr_category(rate: number) {
  if (rate <= 0.15) {
    return ["0-15%", "rgba(11, 156, 49, 0.75", "white"];
  } else if (rate <= 0.45) {
    return ["16-45%", "rgba(255, 158, 0, 0.75)", "black"];
  } else {
    return [">45%", "rgba(230, 0, 0, 0.75)", "white"];
  }
}
