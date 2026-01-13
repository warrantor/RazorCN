namespace TW_Merge;

/// <summary>
/// Configuration for tailwind-merge with class group definitions.
/// Based on Tailwind CSS v4.x class structure.
/// </summary>
public sealed class TwMergeConfig
{
    public Dictionary<string, ClassGroup> ClassGroups { get; } = new();

    /// <summary>
    /// Creates the default configuration matching Tailwind CSS defaults.
    /// </summary>
    public static TwMergeConfig Default()
    {
        var config = new TwMergeConfig();
        config.AddDefaultGroups();
        return config;
    }

    private void Add(ClassGroup group)
    {
        ClassGroups[group.Id] = group;
    }

    private void AddDefaultGroups()
    {
        // ===== LAYOUT =====
        Add(new ClassGroup("display")
            .WithClasses("block", "inline-block", "inline", "flex", "inline-flex",
                "table", "inline-table", "table-caption", "table-cell", "table-column",
                "table-column-group", "table-footer-group", "table-header-group",
                "table-row-group", "table-row", "flow-root", "grid", "inline-grid",
                "contents", "list-item", "hidden"));

        Add(new ClassGroup("position")
            .WithClasses("static", "fixed", "absolute", "relative", "sticky"));

        Add(new ClassGroup("visibility")
            .WithClasses("visible", "invisible", "collapse"));

        Add(new ClassGroup("z")
            .WithValidator(v => v.StartsWith("z-")));

        Add(new ClassGroup("overflow")
            .WithClasses("overflow-auto", "overflow-hidden", "overflow-clip",
                "overflow-visible", "overflow-scroll"));

        Add(new ClassGroup("overflow-x")
            .WithClasses("overflow-x-auto", "overflow-x-hidden", "overflow-x-clip",
                "overflow-x-visible", "overflow-x-scroll"));

        Add(new ClassGroup("overflow-y")
            .WithClasses("overflow-y-auto", "overflow-y-hidden", "overflow-y-clip",
                "overflow-y-visible", "overflow-y-scroll"));

        // ===== FLEXBOX & GRID =====
        Add(new ClassGroup("flex-direction")
            .WithClasses("flex-row", "flex-row-reverse", "flex-col", "flex-col-reverse"));

        Add(new ClassGroup("flex-wrap")
            .WithClasses("flex-wrap", "flex-wrap-reverse", "flex-nowrap"));

        Add(new ClassGroup("flex")
            .WithClasses("flex-1", "flex-auto", "flex-initial", "flex-none")
            .WithValidator(v => v.StartsWith("flex-[") && v.EndsWith("]")));

        Add(new ClassGroup("grow")
            .WithClasses("grow", "grow-0")
            .WithValidator(v => v.StartsWith("grow-")));

        Add(new ClassGroup("shrink")
            .WithClasses("shrink", "shrink-0")
            .WithValidator(v => v.StartsWith("shrink-")));

        Add(new ClassGroup("basis")
            .WithValidator(v => v.StartsWith("basis-")));

        Add(new ClassGroup("justify")
            .WithClasses("justify-normal", "justify-start", "justify-end", "justify-center",
                "justify-between", "justify-around", "justify-evenly", "justify-stretch"));

        Add(new ClassGroup("justify-items")
            .WithClasses("justify-items-start", "justify-items-end", "justify-items-center",
                "justify-items-stretch"));

        Add(new ClassGroup("justify-self")
            .WithClasses("justify-self-auto", "justify-self-start", "justify-self-end",
                "justify-self-center", "justify-self-stretch"));

        Add(new ClassGroup("items")
            .WithClasses("items-start", "items-end", "items-center", "items-baseline",
                "items-stretch"));

        Add(new ClassGroup("content")
            .WithClasses("content-normal", "content-start", "content-end", "content-center",
                "content-between", "content-around", "content-evenly", "content-baseline",
                "content-stretch"));

        Add(new ClassGroup("self")
            .WithClasses("self-auto", "self-start", "self-end", "self-center",
                "self-stretch", "self-baseline"));

        Add(new ClassGroup("gap")
            .WithValidator(v => v.StartsWith("gap-") && !v.StartsWith("gap-x-") && !v.StartsWith("gap-y-"))
            .ConflictsWith("gap-x", "gap-y"));

        Add(new ClassGroup("gap-x")
            .WithValidator(v => v.StartsWith("gap-x-")));

        Add(new ClassGroup("gap-y")
            .WithValidator(v => v.StartsWith("gap-y-")));

        Add(new ClassGroup("grid-cols")
            .WithValidator(v => v.StartsWith("grid-cols-")));

        Add(new ClassGroup("grid-rows")
            .WithValidator(v => v.StartsWith("grid-rows-")));

        // ===== SPACING =====
        Add(new ClassGroup("p")
            .WithValidator(v => v.StartsWith("p-") && !v.StartsWith("px-") && !v.StartsWith("py-") &&
                !v.StartsWith("pt-") && !v.StartsWith("pr-") && !v.StartsWith("pb-") && !v.StartsWith("pl-") &&
                !v.StartsWith("ps-") && !v.StartsWith("pe-"))
            .ConflictsWith("px", "py", "pt", "pr", "pb", "pl", "ps", "pe"));

        Add(new ClassGroup("px")
            .WithValidator(v => v.StartsWith("px-"))
            .ConflictsWith("pr", "pl"));

        Add(new ClassGroup("py")
            .WithValidator(v => v.StartsWith("py-"))
            .ConflictsWith("pt", "pb"));

        Add(new ClassGroup("ps")
            .WithValidator(v => v.StartsWith("ps-")));

        Add(new ClassGroup("pe")
            .WithValidator(v => v.StartsWith("pe-")));

        Add(new ClassGroup("pt")
            .WithValidator(v => v.StartsWith("pt-")));

        Add(new ClassGroup("pr")
            .WithValidator(v => v.StartsWith("pr-")));

        Add(new ClassGroup("pb")
            .WithValidator(v => v.StartsWith("pb-")));

        Add(new ClassGroup("pl")
            .WithValidator(v => v.StartsWith("pl-")));

        Add(new ClassGroup("m")
            .WithValidator(v => v.StartsWith("m-") && !v.StartsWith("mx-") && !v.StartsWith("my-") &&
                !v.StartsWith("mt-") && !v.StartsWith("mr-") && !v.StartsWith("mb-") && !v.StartsWith("ml-") &&
                !v.StartsWith("ms-") && !v.StartsWith("me-"))
            .ConflictsWith("mx", "my", "mt", "mr", "mb", "ml", "ms", "me"));

        Add(new ClassGroup("mx")
            .WithValidator(v => v.StartsWith("mx-"))
            .ConflictsWith("mr", "ml"));

        Add(new ClassGroup("my")
            .WithValidator(v => v.StartsWith("my-"))
            .ConflictsWith("mt", "mb"));

        Add(new ClassGroup("ms")
            .WithValidator(v => v.StartsWith("ms-")));

        Add(new ClassGroup("me")
            .WithValidator(v => v.StartsWith("me-")));

        Add(new ClassGroup("mt")
            .WithValidator(v => v.StartsWith("mt-")));

        Add(new ClassGroup("mr")
            .WithValidator(v => v.StartsWith("mr-")));

        Add(new ClassGroup("mb")
            .WithValidator(v => v.StartsWith("mb-")));

        Add(new ClassGroup("ml")
            .WithValidator(v => v.StartsWith("ml-")));

        Add(new ClassGroup("space-x")
            .WithValidator(v => v.StartsWith("space-x-")));

        Add(new ClassGroup("space-y")
            .WithValidator(v => v.StartsWith("space-y-")));

        // ===== SIZING =====
        Add(new ClassGroup("w")
            .WithValidator(v => v.StartsWith("w-")));

        Add(new ClassGroup("min-w")
            .WithValidator(v => v.StartsWith("min-w-")));

        Add(new ClassGroup("max-w")
            .WithValidator(v => v.StartsWith("max-w-")));

        Add(new ClassGroup("h")
            .WithValidator(v => v.StartsWith("h-")));

        Add(new ClassGroup("min-h")
            .WithValidator(v => v.StartsWith("min-h-")));

        Add(new ClassGroup("max-h")
            .WithValidator(v => v.StartsWith("max-h-")));

        Add(new ClassGroup("size")
            .WithValidator(v => v.StartsWith("size-"))
            .ConflictsWith("w", "h"));

        // ===== TYPOGRAPHY =====
        Add(new ClassGroup("font-family")
            .WithClasses("font-sans", "font-serif", "font-mono")
            .WithValidator(v => v.StartsWith("font-[") && v.EndsWith("]")));

        Add(new ClassGroup("font-size")
            .WithClasses("text-xs", "text-sm", "text-base", "text-lg", "text-xl",
                "text-2xl", "text-3xl", "text-4xl", "text-5xl", "text-6xl",
                "text-7xl", "text-8xl", "text-9xl")
            .WithValidator(v => v.StartsWith("text-[") && v.EndsWith("]") && !v.Contains("#")));

        Add(new ClassGroup("font-weight")
            .WithClasses("font-thin", "font-extralight", "font-light", "font-normal",
                "font-medium", "font-semibold", "font-bold", "font-extrabold", "font-black"));

        Add(new ClassGroup("font-style")
            .WithClasses("italic", "not-italic"));

        Add(new ClassGroup("text-color")
            .WithValidator(v => v.StartsWith("text-") && IsColorClass(v[5..])));

        Add(new ClassGroup("text-align")
            .WithClasses("text-left", "text-center", "text-right", "text-justify",
                "text-start", "text-end"));

        Add(new ClassGroup("text-decoration")
            .WithClasses("underline", "overline", "line-through", "no-underline"));

        Add(new ClassGroup("text-transform")
            .WithClasses("uppercase", "lowercase", "capitalize", "normal-case"));

        Add(new ClassGroup("text-overflow")
            .WithClasses("truncate", "text-ellipsis", "text-clip"));

        Add(new ClassGroup("text-wrap")
            .WithClasses("text-wrap", "text-nowrap", "text-balance", "text-pretty"));

        Add(new ClassGroup("whitespace")
            .WithClasses("whitespace-normal", "whitespace-nowrap", "whitespace-pre",
                "whitespace-pre-line", "whitespace-pre-wrap", "whitespace-break-spaces"));

        Add(new ClassGroup("leading")
            .WithClasses("leading-none", "leading-tight", "leading-snug", "leading-normal",
                "leading-relaxed", "leading-loose")
            .WithValidator(v => v.StartsWith("leading-")));

        Add(new ClassGroup("tracking")
            .WithClasses("tracking-tighter", "tracking-tight", "tracking-normal",
                "tracking-wide", "tracking-wider", "tracking-widest")
            .WithValidator(v => v.StartsWith("tracking-")));

        // ===== BACKGROUNDS =====
        Add(new ClassGroup("bg-color")
            .WithValidator(v => v.StartsWith("bg-") && IsColorClass(v[3..])));

        Add(new ClassGroup("bg-opacity")
            .WithValidator(v => v.StartsWith("bg-opacity-")));

        Add(new ClassGroup("bg-position")
            .WithClasses("bg-bottom", "bg-center", "bg-left", "bg-left-bottom",
                "bg-left-top", "bg-right", "bg-right-bottom", "bg-right-top", "bg-top"));

        Add(new ClassGroup("bg-repeat")
            .WithClasses("bg-repeat", "bg-no-repeat", "bg-repeat-x", "bg-repeat-y",
                "bg-repeat-round", "bg-repeat-space"));

        Add(new ClassGroup("bg-size")
            .WithClasses("bg-auto", "bg-cover", "bg-contain"));

        Add(new ClassGroup("bg-attachment")
            .WithClasses("bg-fixed", "bg-local", "bg-scroll"));

        Add(new ClassGroup("bg-clip")
            .WithClasses("bg-clip-border", "bg-clip-padding", "bg-clip-content", "bg-clip-text"));

        Add(new ClassGroup("bg-origin")
            .WithClasses("bg-origin-border", "bg-origin-padding", "bg-origin-content"));

        Add(new ClassGroup("gradient-from")
            .WithValidator(v => v.StartsWith("from-")));

        Add(new ClassGroup("gradient-via")
            .WithValidator(v => v.StartsWith("via-")));

        Add(new ClassGroup("gradient-to")
            .WithValidator(v => v.StartsWith("to-") && !v.StartsWith("to-t") && !v.StartsWith("to-b") &&
                !v.StartsWith("to-l") && !v.StartsWith("to-r")));

        Add(new ClassGroup("bg-gradient")
            .WithClasses("bg-none", "bg-gradient-to-t", "bg-gradient-to-tr", "bg-gradient-to-r",
                "bg-gradient-to-br", "bg-gradient-to-b", "bg-gradient-to-bl", "bg-gradient-to-l",
                "bg-gradient-to-tl"));

        // ===== BORDERS =====
        Add(new ClassGroup("rounded")
            .WithClasses("rounded-none", "rounded-sm", "rounded", "rounded-md", "rounded-lg",
                "rounded-xl", "rounded-2xl", "rounded-3xl", "rounded-full")
            .WithValidator(v => v.StartsWith("rounded-["))
            .ConflictsWith("rounded-t", "rounded-r", "rounded-b", "rounded-l",
                "rounded-tl", "rounded-tr", "rounded-br", "rounded-bl",
                "rounded-s", "rounded-e", "rounded-ss", "rounded-se", "rounded-es", "rounded-ee"));

        Add(new ClassGroup("rounded-t")
            .WithValidator(v => v.StartsWith("rounded-t"))
            .ConflictsWith("rounded-tl", "rounded-tr"));

        Add(new ClassGroup("rounded-r")
            .WithValidator(v => v.StartsWith("rounded-r"))
            .ConflictsWith("rounded-tr", "rounded-br"));

        Add(new ClassGroup("rounded-b")
            .WithValidator(v => v.StartsWith("rounded-b"))
            .ConflictsWith("rounded-bl", "rounded-br"));

        Add(new ClassGroup("rounded-l")
            .WithValidator(v => v.StartsWith("rounded-l"))
            .ConflictsWith("rounded-tl", "rounded-bl"));

        Add(new ClassGroup("rounded-tl")
            .WithValidator(v => v.StartsWith("rounded-tl-")));

        Add(new ClassGroup("rounded-tr")
            .WithValidator(v => v.StartsWith("rounded-tr-")));

        Add(new ClassGroup("rounded-br")
            .WithValidator(v => v.StartsWith("rounded-br-")));

        Add(new ClassGroup("rounded-bl")
            .WithValidator(v => v.StartsWith("rounded-bl-")));

        Add(new ClassGroup("border-w")
            .WithClasses("border", "border-0", "border-2", "border-4", "border-8")
            .WithValidator(v => v.StartsWith("border-[") && !v.Contains("#"))
            .ConflictsWith("border-t", "border-r", "border-b", "border-l", "border-x", "border-y"));

        Add(new ClassGroup("border-x")
            .WithClasses("border-x", "border-x-0", "border-x-2", "border-x-4", "border-x-8")
            .ConflictsWith("border-l", "border-r"));

        Add(new ClassGroup("border-y")
            .WithClasses("border-y", "border-y-0", "border-y-2", "border-y-4", "border-y-8")
            .ConflictsWith("border-t", "border-b"));

        Add(new ClassGroup("border-t")
            .WithClasses("border-t", "border-t-0", "border-t-2", "border-t-4", "border-t-8"));

        Add(new ClassGroup("border-r")
            .WithClasses("border-r", "border-r-0", "border-r-2", "border-r-4", "border-r-8"));

        Add(new ClassGroup("border-b")
            .WithClasses("border-b", "border-b-0", "border-b-2", "border-b-4", "border-b-8"));

        Add(new ClassGroup("border-l")
            .WithClasses("border-l", "border-l-0", "border-l-2", "border-l-4", "border-l-8"));

        Add(new ClassGroup("border-color")
            .WithValidator(v => v.StartsWith("border-") && IsColorClass(v[7..])));

        Add(new ClassGroup("border-style")
            .WithClasses("border-solid", "border-dashed", "border-dotted", "border-double", "border-hidden", "border-none"));

        Add(new ClassGroup("divide-x")
            .WithClasses("divide-x", "divide-x-0", "divide-x-2", "divide-x-4", "divide-x-8", "divide-x-reverse"));

        Add(new ClassGroup("divide-y")
            .WithClasses("divide-y", "divide-y-0", "divide-y-2", "divide-y-4", "divide-y-8", "divide-y-reverse"));

        Add(new ClassGroup("divide-color")
            .WithValidator(v => v.StartsWith("divide-") && IsColorClass(v[7..])));

        Add(new ClassGroup("divide-style")
            .WithClasses("divide-solid", "divide-dashed", "divide-dotted", "divide-double", "divide-none"));

        Add(new ClassGroup("outline-style")
            .WithClasses("outline-none", "outline", "outline-dashed", "outline-dotted", "outline-double"));

        Add(new ClassGroup("outline-w")
            .WithClasses("outline-0", "outline-1", "outline-2", "outline-4", "outline-8"));

        Add(new ClassGroup("outline-offset")
            .WithValidator(v => v.StartsWith("outline-offset-")));

        Add(new ClassGroup("outline-color")
            .WithValidator(v => v.StartsWith("outline-") && IsColorClass(v[8..])));

        Add(new ClassGroup("ring-w")
            .WithClasses("ring", "ring-0", "ring-1", "ring-2", "ring-4", "ring-8", "ring-inset"));

        Add(new ClassGroup("ring-color")
            .WithValidator(v => v.StartsWith("ring-") && IsColorClass(v[5..])));

        Add(new ClassGroup("ring-offset-w")
            .WithClasses("ring-offset-0", "ring-offset-1", "ring-offset-2", "ring-offset-4", "ring-offset-8"));

        Add(new ClassGroup("ring-offset-color")
            .WithValidator(v => v.StartsWith("ring-offset-") && IsColorClass(v[12..])));

        // ===== EFFECTS =====
        Add(new ClassGroup("shadow")
            .WithClasses("shadow-sm", "shadow", "shadow-md", "shadow-lg", "shadow-xl",
                "shadow-2xl", "shadow-inner", "shadow-none"));

        Add(new ClassGroup("shadow-color")
            .WithValidator(v => v.StartsWith("shadow-") && IsColorClass(v[7..])));

        Add(new ClassGroup("opacity")
            .WithValidator(v => v.StartsWith("opacity-")));

        Add(new ClassGroup("mix-blend")
            .WithClasses("mix-blend-normal", "mix-blend-multiply", "mix-blend-screen",
                "mix-blend-overlay", "mix-blend-darken", "mix-blend-lighten",
                "mix-blend-color-dodge", "mix-blend-color-burn", "mix-blend-hard-light",
                "mix-blend-soft-light", "mix-blend-difference", "mix-blend-exclusion",
                "mix-blend-hue", "mix-blend-saturation", "mix-blend-color",
                "mix-blend-luminosity", "mix-blend-plus-lighter", "mix-blend-plus-darker"));

        Add(new ClassGroup("bg-blend")
            .WithClasses("bg-blend-normal", "bg-blend-multiply", "bg-blend-screen",
                "bg-blend-overlay", "bg-blend-darken", "bg-blend-lighten",
                "bg-blend-color-dodge", "bg-blend-color-burn", "bg-blend-hard-light",
                "bg-blend-soft-light", "bg-blend-difference", "bg-blend-exclusion",
                "bg-blend-hue", "bg-blend-saturation", "bg-blend-color", "bg-blend-luminosity"));

        // ===== FILTERS =====
        Add(new ClassGroup("blur")
            .WithClasses("blur-none", "blur-sm", "blur", "blur-md", "blur-lg", "blur-xl",
                "blur-2xl", "blur-3xl"));

        Add(new ClassGroup("brightness")
            .WithValidator(v => v.StartsWith("brightness-")));

        Add(new ClassGroup("contrast")
            .WithValidator(v => v.StartsWith("contrast-")));

        Add(new ClassGroup("grayscale")
            .WithClasses("grayscale-0", "grayscale"));

        Add(new ClassGroup("hue-rotate")
            .WithValidator(v => v.StartsWith("hue-rotate-")));

        Add(new ClassGroup("invert")
            .WithClasses("invert-0", "invert"));

        Add(new ClassGroup("saturate")
            .WithValidator(v => v.StartsWith("saturate-")));

        Add(new ClassGroup("sepia")
            .WithClasses("sepia-0", "sepia"));

        Add(new ClassGroup("drop-shadow")
            .WithClasses("drop-shadow-sm", "drop-shadow", "drop-shadow-md", "drop-shadow-lg",
                "drop-shadow-xl", "drop-shadow-2xl", "drop-shadow-none"));

        Add(new ClassGroup("backdrop-blur")
            .WithValidator(v => v.StartsWith("backdrop-blur-")));

        Add(new ClassGroup("backdrop-brightness")
            .WithValidator(v => v.StartsWith("backdrop-brightness-")));

        Add(new ClassGroup("backdrop-contrast")
            .WithValidator(v => v.StartsWith("backdrop-contrast-")));

        Add(new ClassGroup("backdrop-grayscale")
            .WithClasses("backdrop-grayscale-0", "backdrop-grayscale"));

        Add(new ClassGroup("backdrop-hue-rotate")
            .WithValidator(v => v.StartsWith("backdrop-hue-rotate-")));

        Add(new ClassGroup("backdrop-invert")
            .WithClasses("backdrop-invert-0", "backdrop-invert"));

        Add(new ClassGroup("backdrop-opacity")
            .WithValidator(v => v.StartsWith("backdrop-opacity-")));

        Add(new ClassGroup("backdrop-saturate")
            .WithValidator(v => v.StartsWith("backdrop-saturate-")));

        Add(new ClassGroup("backdrop-sepia")
            .WithClasses("backdrop-sepia-0", "backdrop-sepia"));

        // ===== TRANSITIONS & ANIMATION =====
        Add(new ClassGroup("transition")
            .WithClasses("transition-none", "transition-all", "transition",
                "transition-colors", "transition-opacity", "transition-shadow",
                "transition-transform"));

        Add(new ClassGroup("duration")
            .WithValidator(v => v.StartsWith("duration-")));

        Add(new ClassGroup("ease")
            .WithClasses("ease-linear", "ease-in", "ease-out", "ease-in-out"));

        Add(new ClassGroup("delay")
            .WithValidator(v => v.StartsWith("delay-")));

        Add(new ClassGroup("animate")
            .WithClasses("animate-none", "animate-spin", "animate-ping",
                "animate-pulse", "animate-bounce"));

        // ===== TRANSFORMS =====
        Add(new ClassGroup("scale")
            .WithValidator(v => v.StartsWith("scale-") && !v.StartsWith("scale-x-") && !v.StartsWith("scale-y-"))
            .ConflictsWith("scale-x", "scale-y"));

        Add(new ClassGroup("scale-x")
            .WithValidator(v => v.StartsWith("scale-x-")));

        Add(new ClassGroup("scale-y")
            .WithValidator(v => v.StartsWith("scale-y-")));

        Add(new ClassGroup("rotate")
            .WithValidator(v => v.StartsWith("rotate-")));

        Add(new ClassGroup("translate-x")
            .WithValidator(v => v.StartsWith("translate-x-")));

        Add(new ClassGroup("translate-y")
            .WithValidator(v => v.StartsWith("translate-y-")));

        Add(new ClassGroup("skew-x")
            .WithValidator(v => v.StartsWith("skew-x-")));

        Add(new ClassGroup("skew-y")
            .WithValidator(v => v.StartsWith("skew-y-")));

        Add(new ClassGroup("transform-origin")
            .WithClasses("origin-center", "origin-top", "origin-top-right", "origin-right",
                "origin-bottom-right", "origin-bottom", "origin-bottom-left", "origin-left",
                "origin-top-left"));

        // ===== INTERACTIVITY =====
        Add(new ClassGroup("cursor")
            .WithClasses("cursor-auto", "cursor-default", "cursor-pointer", "cursor-wait",
                "cursor-text", "cursor-move", "cursor-help", "cursor-not-allowed", "cursor-none",
                "cursor-context-menu", "cursor-progress", "cursor-cell", "cursor-crosshair",
                "cursor-vertical-text", "cursor-alias", "cursor-copy", "cursor-no-drop",
                "cursor-grab", "cursor-grabbing", "cursor-all-scroll", "cursor-col-resize",
                "cursor-row-resize", "cursor-n-resize", "cursor-e-resize", "cursor-s-resize",
                "cursor-w-resize", "cursor-ne-resize", "cursor-nw-resize", "cursor-se-resize",
                "cursor-sw-resize", "cursor-ew-resize", "cursor-ns-resize", "cursor-nesw-resize",
                "cursor-nwse-resize", "cursor-zoom-in", "cursor-zoom-out"));

        Add(new ClassGroup("pointer-events")
            .WithClasses("pointer-events-none", "pointer-events-auto"));

        Add(new ClassGroup("resize")
            .WithClasses("resize-none", "resize-y", "resize-x", "resize"));

        Add(new ClassGroup("select")
            .WithClasses("select-none", "select-text", "select-all", "select-auto"));

        Add(new ClassGroup("scroll-behavior")
            .WithClasses("scroll-auto", "scroll-smooth"));

        Add(new ClassGroup("touch")
            .WithClasses("touch-auto", "touch-none", "touch-pan-x", "touch-pan-left",
                "touch-pan-right", "touch-pan-y", "touch-pan-up", "touch-pan-down",
                "touch-pinch-zoom", "touch-manipulation"));

        Add(new ClassGroup("appearance")
            .WithClasses("appearance-none", "appearance-auto"));

        Add(new ClassGroup("caret-color")
            .WithValidator(v => v.StartsWith("caret-")));

        Add(new ClassGroup("accent-color")
            .WithValidator(v => v.StartsWith("accent-")));

        Add(new ClassGroup("will-change")
            .WithClasses("will-change-auto", "will-change-scroll", "will-change-contents",
                "will-change-transform"));

        // ===== POSITION =====
        Add(new ClassGroup("inset")
            .WithValidator(v => v.StartsWith("inset-") && !v.StartsWith("inset-x-") && !v.StartsWith("inset-y-"))
            .ConflictsWith("inset-x", "inset-y", "top", "right", "bottom", "left", "start", "end"));

        Add(new ClassGroup("inset-x")
            .WithValidator(v => v.StartsWith("inset-x-"))
            .ConflictsWith("left", "right"));

        Add(new ClassGroup("inset-y")
            .WithValidator(v => v.StartsWith("inset-y-"))
            .ConflictsWith("top", "bottom"));

        Add(new ClassGroup("top")
            .WithValidator(v => v.StartsWith("top-")));

        Add(new ClassGroup("right")
            .WithValidator(v => v.StartsWith("right-")));

        Add(new ClassGroup("bottom")
            .WithValidator(v => v.StartsWith("bottom-")));

        Add(new ClassGroup("left")
            .WithValidator(v => v.StartsWith("left-")));

        Add(new ClassGroup("start")
            .WithValidator(v => v.StartsWith("start-")));

        Add(new ClassGroup("end")
            .WithValidator(v => v.StartsWith("end-")));

        // ===== SVG =====
        Add(new ClassGroup("fill")
            .WithClasses("fill-none", "fill-current")
            .WithValidator(v => v.StartsWith("fill-")));

        Add(new ClassGroup("stroke")
            .WithClasses("stroke-none", "stroke-current")
            .WithValidator(v => v.StartsWith("stroke-") && !v.StartsWith("stroke-w-")));

        Add(new ClassGroup("stroke-w")
            .WithClasses("stroke-0", "stroke-1", "stroke-2")
            .WithValidator(v => v.StartsWith("stroke-w-")));

        // ===== ACCESSIBILITY =====
        Add(new ClassGroup("sr")
            .WithClasses("sr-only", "not-sr-only"));

        Add(new ClassGroup("forced-color-adjust")
            .WithClasses("forced-color-adjust-auto", "forced-color-adjust-none"));
    }

    private static bool IsColorClass(string value)
    {
        if (string.IsNullOrEmpty(value)) return false;

        // Arbitrary color
        if (value.StartsWith('[') && value.EndsWith(']')) return true;

        // Named colors with optional shade and opacity
        var colorNames = new HashSet<string>
        {
            "inherit", "current", "transparent", "black", "white",
            "slate", "gray", "zinc", "neutral", "stone",
            "red", "orange", "amber", "yellow", "lime",
            "green", "emerald", "teal", "cyan", "sky",
            "blue", "indigo", "violet", "purple", "fuchsia",
            "pink", "rose",
            // Theme colors
            "primary", "secondary", "accent", "muted", "destructive",
            "background", "foreground", "card", "popover", "border",
            "input", "ring"
        };

        // Check for color-shade pattern (e.g., red-500)
        var parts = value.Split('/')[0].Split('-'); // Remove opacity modifier first
        return colorNames.Contains(parts[0]);
    }
}
