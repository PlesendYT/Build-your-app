import { Command } from "commander";

const program = new Command();
program.name("demo").description("Demo CLI").version("1.0.0");

program
  .command("greet")
  .description("Print a greeting")
  .option("-n, --name <name>", "name to greet", "World")
  .action((opts) => {
    console.log(`Hello, ${opts.name}!`);
  });

program
  .command("add")
  .description("Sum numbers")
  .argument("<numbers...>")
  .action((nums) => {
    const total = nums.map(Number).reduce((a, b) => a + b, 0);
    console.log(`sum = ${total}`);
  });

program.parseAsync().catch((err) => {
  console.error(err);
  process.exit(1);
});